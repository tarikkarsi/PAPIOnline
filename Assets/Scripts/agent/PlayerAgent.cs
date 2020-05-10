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
        private TextMeshPro rewardText;

        private PlayerMetrics previousPlayerMetrics = new PlayerMetrics();
        private PlayerMetrics previousEnemyMetrics = new PlayerMetrics();

        private volatile bool requestDecision = true;

        public PlayerAgent(String name, PlayerProperties playerProperties, ISkill[] skills, bool requestDecision = true)
        {
            // Initialize player
            this.player = new Player(name, playerProperties, skills);
            this.requestDecision = requestDecision;
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
            this.rewardText = battleArena.GetRewardText(tag);
            // Initialize player rewards
            this.rewards = new PlayerRewards(this.player.GetName(), MaxStep);
        }

        public IPlayer GetPlayer()
        {
            return this.player;
        }

        public void FixedUpdate()
        {
            // Update position
            player.SetPosition(transform.position);
            // Update timers
            player.UpdatePlayer(Time.fixedDeltaTime);
        }

        public override void OnEpisodeBegin()
        {
			// Set position relative to battle arena
            Vector3 position = CompareTag(BattleArena.AGENT_BLUE_TAG) ? new Vector3(30f, 0f, 30f) : new Vector3(-30f, 0f, -30f);
            position += battleArena.GetPosition();
            this.transform.position = position;

            // Reset wrapped player
            this.player.ResetPlayer();
            this.player.SetPosition(transform.position);
			// Request the first decision at the beggining of the episode
            RequestDecision();
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            // Debug.Log("Agent Move Action1 " + vectorAction[0]);
            // Debug.Log("Agent Skill Action2 " + vectorAction[1]);
            // Debug.Log("Agent Potion Action2 " + vectorAction[2]);

            // Save current player and enemy metrics before any action
            SaveCurrentMetrics();

            // Make move actions
            MoveAction(Mathf.FloorToInt(vectorAction[0]));

            // Make skill and attack actions
            SkillAction(Mathf.FloorToInt(vectorAction[1]), enemy);

            // Make potion actions
            PotionAction(Mathf.FloorToInt(vectorAction[2]));

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
            // Player Properties
            // Position information
            sensor.AddObservation(player.GetPosition());

            // Health capacity
            sensor.AddObservation(player.GetHealthCapacity());

            // Health information
            sensor.AddObservation(player.GetHealth());

            // Health potion count
            sensor.AddObservation(player.GetHealthPotionCount());

            // Mana capacity
            sensor.AddObservation(player.GetManaCapacity());

            // Mana information
            sensor.AddObservation(player.GetMana());

            // Mana potion count
            sensor.AddObservation(player.GetManaPotionCount());

            // Speed information
            sensor.AddObservation(player.GetSpeed());

            // Defense information
            sensor.AddObservation(player.GetDefense());

            // Damage information
            sensor.AddObservation(player.GetDamage());

			// Attack range information
            sensor.AddObservation(player.GetAttackRange());

            // Stun information
            sensor.AddObservation(player.IsStunned());

            // Attack usage information
            sensor.AddObservation(player.IsAttacking());

            // Skill information
            IAttackSkill attackSkill;
			IBuffSkill buffSkill;
            foreach (ISkill skill in player.GetSkills())
            {
                sensor.AddObservation(skill.IsAvailable());
                sensor.AddObservation((int)skill.GetSkillKind());
                sensor.AddObservation(skill.GetManaConsumption());
                sensor.AddObservation(skill.GetTimeout());

				// attack and buff skill observations should be equal to preserve total observation count
				// observation for attack skill
				if (skill.GetSkillKind() == SkillKind.ATTACK)
				{
                    attackSkill = (IAttackSkill)skill;
                    sensor.AddObservation(attackSkill.GetDamage());
                    sensor.AddObservation(attackSkill.GetRange());
                    sensor.AddObservation(attackSkill.HasDebuff());
                    sensor.AddObservation(attackSkill.GetDebuffPercentage());
                }
				// observation for buff skill
				else
				{
                    buffSkill = (IBuffSkill)skill;
                    sensor.AddObservation(buffSkill.GetDuration());
                    sensor.AddObservation(buffSkill.GetAmount());
                    sensor.AddObservation((int)buffSkill.GetBuffKind());
                    sensor.AddObservation(buffSkill.IsPeriodic());
                }
            }

            // Distance between enemy
            sensor.AddObservation(Utils.GetDistance(player, enemy));          
         
            // Enemy properties
            // Position information
            sensor.AddObservation(enemy.GetPosition());

            // debuff information
			// maximum three debuff information
            int debuffCount = enemy.GetAppliedDebuffs().Count;
            for (int i = 0; i < 3; i++)
			{
                sensor.AddObservation(i < debuffCount ? (int)enemy.GetAppliedDebuffs()[i].GetBuffKind() : -1);
			}
        }

        public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
        {
            // Set actions masks
            actionMasker.SetMask(MOVE_BRANCH_INDEX, Utils.GetMoveMasks(player));
            actionMasker.SetMask(SKILL_BRANCH_INDEX, Utils.GetSkillMasks(player, enemy));
            actionMasker.SetMask(POTION_BRANCH_INDEX, Utils.GetPotionMasks(player));
        }

        private void SaveCurrentMetrics()
        {
            previousPlayerMetrics.Set(player);
            previousEnemyMetrics.Set(enemy);
            rewardText.text = GetCumulativeReward().ToString("0.00000");
        }

        private void GiveRewards()
        {
            if (enemy.IsDead())
            {
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
                // Reward for damaging the enemy (covers using skills, attacking and health debuffs)
                AddReward(rewards.GetDamageReward(previousEnemyMetrics.properties, enemy.GetProperties()));

                // Reward for increasing player properties (covers using buffs and potions)
                AddReward(rewards.GetPropertyReward(previousPlayerMetrics.properties, player.GetProperties()));

                // Reward for debuffing the enemy (covers using debuffs)
                AddReward(rewards.GetDebuffReward(previousEnemyMetrics.debuffs, enemy.GetAppliedDebuffs()));

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
                transform.Rotate(rotation, Time.deltaTime * 200f);
                agentRB.AddForce(direction * 2f, ForceMode.VelocityChange);
                player.SetPosition(transform.position);
            }
        }

        public void SkillAction(int action, IPlayer enemy)
        {
            // 0 means idle
            // 1 - skill count, means using skill
            if (action > 0 && action < player.GetSkillCount() + 1)
            {
                player.UseSkill(action - 1, enemy);
            }
            // Skill count + 1, means normal attack
            else if (action == player.GetSkillCount() + 1)
            {
                player.Attack(enemy);
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
        }

    }

}