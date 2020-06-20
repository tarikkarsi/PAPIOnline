/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          PlayerAgent
 *   
 *   Description:    Player agent base class
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using TMPro;

namespace PAPIOnline
{

    public class PlayerAgent : Agent
    {
        public readonly static int MOVE_BRANCH_INDEX = 0;
        public readonly static int SKILL_BRANCH_INDEX = 1;
        public readonly static int POTION_BRANCH_INDEX = 2;

        protected BattleArena battleArena;
        protected IPlayer player;
        protected IPlayer enemy;
        private PlayerRewards rewards;

        private Rigidbody agentRB;
        private BattleInfo battleInfo;

        private PlayerMetrics previousPlayerMetrics = new PlayerMetrics();
        private PlayerMetrics previousEnemyMetrics = new PlayerMetrics();

        private bool requestDecision = true;
        private bool manualReward = true;
        private bool isPaused = false;
        
        protected int[] skillMasks;

        public PlayerAgent(String name, PlayerProperties playerProperties, ISkill[] skills, bool requestDecision = true, bool manualReward = true)
        {
            // Initialize player
            this.player = new Player(name, playerProperties, skills);
            this.requestDecision = requestDecision;
            this.manualReward = manualReward;
        }

        public virtual void Start()
        {
            // Set name
            this.player.SetName(tag);
            // Get rigit body
            this.agentRB = GetComponent<Rigidbody>();
            // Initialize enemy
            this.battleArena = GetComponentInParent<BattleArena>();
            this.enemy = battleArena.GetRival(tag).GetPlayer();
            this.battleInfo = battleArena.GetBattleInfo(tag);
            // Initialize player rewards
            this.rewards = new PlayerRewards(this.player.GetName(), MaxStep);
        }

        public IPlayer GetPlayer()
        {
            return this.player;
        }

        public void FixedUpdate()
        {
            if (!this.isPaused)
            {
                // Update timers
                player.UpdatePlayer(Time.fixedDeltaTime);
            }
        }

        public void PauseAgent()
        {
            this.isPaused = true;
        }

        public void ResumeAgent()
        {
            this.isPaused = false;
        }

        public override void OnEpisodeBegin()
        {
            // Increase play count
            this.battleInfo.IncreasePlayCount();
			// Set position relative to battle arena
            Vector3 position = CompareTag(BattleArena.BLUE_AGENT_TAG) ? new Vector3(20f, 0f, 20f) : new Vector3(-20f, 0f, -20f);
            position += battleArena.GetPosition();
            this.transform.position = position;

            // Reset wrapped player
            this.player.ResetPlayer();
            this.player.SetPosition(transform.position);
			// Request the first decision at the beggining of the episode
            RequestDecision();
        }

        public override void Heuristic(float[] actionsOut)
        {
            // Reset values
            actionsOut[MOVE_BRANCH_INDEX] = 0f;
            actionsOut[SKILL_BRANCH_INDEX] = 0f;
            actionsOut[POTION_BRANCH_INDEX] = 0f;
            // Movements
            if (Input.GetKey(KeyCode.W))
            {
                actionsOut[MOVE_BRANCH_INDEX] = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                actionsOut[MOVE_BRANCH_INDEX] = 2f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                actionsOut[MOVE_BRANCH_INDEX] = 3f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                actionsOut[MOVE_BRANCH_INDEX] = 4f;
            }
            // Skills
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 1f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 2f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 3f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 4f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 5f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 6f;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 7f;
            }
            // Attack
            if (Input.GetKey(KeyCode.Space))
            {
                actionsOut[SKILL_BRANCH_INDEX] = 8f;
            }
            // Potions
            if (Input.GetKeyDown(KeyCode.E))
            {
                actionsOut[POTION_BRANCH_INDEX] = 1f;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                actionsOut[POTION_BRANCH_INDEX] = 2f;
            }
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            //Debug.Log("Agent Move Action " + vectorAction[0]);
            //Debug.Log("Agent Skill Action " + vectorAction[1]);
            //Debug.Log("Agent Potion Action " + vectorAction[2]);

            // Save current player and enemy metrics before any action
            SaveCurrentMetrics();

            // Make move actions
            MoveAction((int) vectorAction[0]);

            // Make skill and attack actions
            SkillAction((int) vectorAction[1], enemy);

            // Make potion actions
            PotionAction((int) vectorAction[2]);

            // Give suitable rewards for this state
            GiveRewards();

            // Request decision after each action
            if (requestDecision)
            {
                RequestDecision();
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // NORMALIZE ALL OBSERVATIONS, BECAUSE TENSORFLOW CANNOT NORMALIZE PROPERLY!!!
            // Player Properties
            // Position information
            sensor.AddObservation((player.GetPosition().x - battleArena.GetPosition().x) / BattleArena.WIDTH);
            sensor.AddObservation((player.GetPosition().z - battleArena.GetPosition().z) / BattleArena.HEIGHT);

            // Health information
            sensor.AddObservation(player.GetHealth() / PlayerProperties.MAX_HEALTH);

            // Health potion count
            sensor.AddObservation(player.GetHealthPotionCount() / PlayerProperties.MAX_HEALTH_POTION);

            // Mana information
            sensor.AddObservation(player.GetMana() / PlayerProperties.MAX_MANA);

            // Mana potion count
            sensor.AddObservation(player.GetManaPotionCount() / PlayerProperties.MAX_MANA_POTION);

            // Speed information
            sensor.AddObservation(player.GetSpeed() / PlayerProperties.MAX_SPEED);

            // Defense information
            sensor.AddObservation(player.GetDefense() / PlayerProperties.MAX_DEFENSE);

            // Damage information
            sensor.AddObservation(player.GetDamage() / PlayerProperties.MAX_DAMAGE);

			// Attack range information
            sensor.AddObservation(player.GetAttackRange() / PlayerProperties.MAX_ATTACK_RANGE);

            // Stun information
            sensor.AddObservation(player.IsStunned());

            // Attack usage information
            sensor.AddObservation(player.IsAttacking());

            // Skill information
            IAttackSkill attackSkill;
			IBuffSkill buffSkill;
            IBuffSkill debuffSkill;
            foreach (ISkill skill in player.GetSkills())
            {
                sensor.AddObservation(skill.IsAvailable());
                sensor.AddOneHotObservation((int)skill.GetSkillKind(), 3);
                sensor.AddObservation(skill.GetManaConsumption() / SkillProperties.MAX_MANA_CONSUMPTION);
                sensor.AddObservation(skill.GetTimeout() / SkillProperties.MAX_TIMEOUT);

				// attack and buff skill observations should be equal to preserve total observation count
				// observation for attack skill
				if (skill.GetSkillKind() == SkillKind.ATTACK)
				{
                    attackSkill = (IAttackSkill)skill;
                    debuffSkill = attackSkill.GetDebuff();
                    sensor.AddObservation(attackSkill.GetDamage() / SkillProperties.MAX_DAMAGE);
                    sensor.AddObservation(attackSkill.GetRange() / SkillProperties.MAX_RANGE);
                    sensor.AddObservation(attackSkill.HasDebuff());
                    sensor.AddObservation(attackSkill.GetDebuffPercentage() / SkillProperties.MAX_DEBUFF_PERCENTAGE);
                    sensor.AddOneHotObservation(debuffSkill != null ? (int) debuffSkill.GetBuffKind() : -1, BuffKindExtensions.Count);
                }
				// observation for buff skill, added one dummy observation to be equal to attack skill observations
				else
				{
                    buffSkill = (IBuffSkill)skill;
                    sensor.AddObservation(buffSkill.GetDuration() / SkillProperties.MAX_DURATION);
                    sensor.AddObservation(buffSkill.GetAmount() / SkillProperties.MAX_AMOUNT);
                    sensor.AddOneHotObservation((int)buffSkill.GetBuffKind(), BuffKindExtensions.Count);
                    sensor.AddObservation(buffSkill.IsPeriodic());
                    sensor.AddObservation(0);
                }
            }

            // Distance between enemy
            sensor.AddObservation(Utils.GetDistance(player, enemy) / BattleArena.MAX_DISTANCE);          
         
            // Enemy properties
            // Position information
            sensor.AddObservation((enemy.GetPosition().x - battleArena.GetPosition().x) / BattleArena.WIDTH);
            sensor.AddObservation((enemy.GetPosition().z - battleArena.GetPosition().z) / BattleArena.HEIGHT);

            // debuff information
			// maximum three debuff information
            IList<IBuffSkill> debuffs = enemy.GetAppliedDebuffs();
            for (int i = 0; i < 3; i++)
			{
                sensor.AddOneHotObservation(i < debuffs.Count ? (int)debuffs[i].GetBuffKind() : -1, BuffKindExtensions.Count);
			}
        }

        public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
        {
            // Set actions masks
            actionMasker.SetMask(MOVE_BRANCH_INDEX, Utils.GetMoveMasks(this, battleArena));
            skillMasks = Utils.GetSkillMasks(player, enemy);
            actionMasker.SetMask(SKILL_BRANCH_INDEX, skillMasks);
            actionMasker.SetMask(POTION_BRANCH_INDEX, Utils.GetPotionMasks(player));
        }

        private void SaveCurrentMetrics()
        {
            previousPlayerMetrics.Set(player);
            previousEnemyMetrics.Set(enemy);
            battleInfo.UpdateInfo(player, enemy, GetCumulativeReward());
        }

        private void GiveRewards()
        {
            if (enemy.IsDead())
            {
                // Increase win count
                this.battleInfo.IncreaseWinCount();
                // Reward for win
                SetReward(rewards.GetWinReward());
                EndEpisode();
            }
            else if (player.IsDead())
            {
                // Reward for loose
                SetReward(rewards.GetLoseReward());
                EndEpisode();
            }
            else
            {
                if (this.manualReward)
                {
                    // Reward for damaging the enemy (covers using skills, attacking and health debuffs)
                    AddReward(rewards.GetDamageReward(previousEnemyMetrics.properties, enemy.GetProperties()));

                    // Reward for increasing player properties (covers using buffs and potions)
                    AddReward(rewards.GetPropertyReward(previousPlayerMetrics.properties, player.GetProperties()));

                    // Reward for debuffing the enemy (covers using debuffs)
                    AddReward(rewards.GetDebuffReward(previousEnemyMetrics.debuffs, enemy.GetAppliedDebuffs()));
                }
                
                // Tiny negative reward every step
                AddReward(rewards.GetStepReward());
            }
        }

        protected void MoveAction(int action)
        {
            Vector3 direction = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            switch (action)
            {
                case 1:
                    direction = transform.forward * 1f;
                    break;
                case 2:
                    direction = transform.forward * -1f;
                    break;
                case 3:
                    rotation = transform.up * 1f;
                    break;
                case 4:
                    rotation = transform.up * -1f;
                    break;
            }

            // Zero means no move
            if (action != 0)
            {
                // Rotate player
                transform.Rotate(rotation, Time.fixedDeltaTime * 360f);
                // Move player and update transform position, we manually moved the transform instead of adding force.
                // Because we are pausing and resuming game. Physics engine should not be affected.
                this.player.Move(direction);
                this.transform.position = player.GetPosition();
            }
            if (action > 4)
            {
                UnityEngine.Debug.LogError(player.GetName() + " move action out of bound " + action);
            }
        }

        public void SkillAction(int action, IPlayer enemy)
        {
            // 0 means idle
            // 1 - skill count, means using skill
            if (action > 0 && action < player.GetSkillCount() + 1)
            {
                bool result = player.UseSkill(action - 1, enemy);
                if (!result)
                {
                    UnityEngine.Debug.LogError(player.GetName() + " skill masks " + string.Join(",", skillMasks));
                    UnityEngine.Debug.LogError("IsStunned: " + player.IsStunned() + " IsAttacking: " + player.IsAttacking() + " IsAnimating: " + player.IsUsingSkill() + " IsDead: " + player.IsDead());
                }
            }
            // Skill count + 1, means normal attack
            else if (action == player.GetSkillCount() + 1)
            {
                player.Attack(enemy);
            }
            else if (action != 0)
            {
                UnityEngine.Debug.LogError(player.GetName() + " skill action out of bound " + action);
            }
        }

        public void PotionAction(int action)
        {
            // 0 means idle
            // 1 means use health potion
            if (action == 1)
            {
                player.UseHealthPotion();
            }
            // 2 means use mana potion
            else if (action == 2)
            {
                player.UseManaPotion();
            }
            else if (action != 0)
            {
                UnityEngine.Debug.LogError(player.GetName() + " potion action out of bound " + action);
            }
        }

    }

}