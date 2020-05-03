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
using MLAgents;
using MLAgents.Sensors;
using TMPro;

namespace PAPIOnline
{

    public class PlayerAgent : Agent
    {
        public readonly static int MOVE_BRANCH_INDEX = 0;
        public readonly static int SKILL_BRANCH_INDEX = 1;
        public readonly static int POTION_BRANCH_INDEX = 2;

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
            BattleArena battleArena = GetComponentInParent<BattleArena>();
            this.enemy = battleArena.GetRival(tag).GetPlayer();
            this.rewardText = battleArena.GetRewardText(tag);
            // Initialize player rewards
            this.rewards = new PlayerRewards(this.player.GetName(), maxStep);
        }

        public IPlayer GetPlayer()
        {
            return this.player;
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
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
            player.ResetPlayer();
            // Initialize position
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
            // Distance between enemy
            sensor.AddObservation(Utils.GetDistance(player, enemy));

            // Position of player and enemy
            sensor.AddObservation(player.GetPosition());
            sensor.AddObservation(enemy.GetPosition());

            // Skill and attack usage information
            sensor.AddObservation(GetSkillObservations());

            // Attack usage information
            sensor.AddObservation(player.IsAttacking());

            // Health capacity
            sensor.AddObservation(player.GetHealthCapacity());

            // Health information
            sensor.AddObservation(player.GetHealth());

            // Mana capacity
            sensor.AddObservation(player.GetManaCapacity());

            // Mana information
            sensor.AddObservation(player.GetMana());

            // Buff count
            sensor.AddObservation(player.GetAppliedBuffs().Count);

            // Enemy debuff count
            sensor.AddObservation(enemy.GetAppliedDebuffs().Count);

            // Health potion count
            sensor.AddObservation(player.GetHealthPotionCount());

            // Mana potion count
            sensor.AddObservation(player.GetManaPotionCount());

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

        private float[] GetSkillObservations()
        {
            ISkill[] skills = player.GetSkills();
            float[] skillObservations = new float[skills.Length];
            for (int i = 0; i < skills.Length; i++)
            {
                skillObservations[i] = skills[i].IsAvailable() ? 0f : 1f;
            }
            return skillObservations;
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