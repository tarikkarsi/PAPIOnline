using System;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using TMPro;

namespace PAPIOnline
{

    public class PlayerAgent : Agent
    {
        private static int MOVE_BRANCH_INDEX = 0;
        private static int SKILL_BRANCH_INDEX = 1;
        private static int POTION_BRANCH_INDEX = 2;

        protected IPlayer player;
        protected IPlayer enemy;
        private PlayerRewards rewards;

        private Rigidbody agentRB;
        private TextMeshPro rewardText;

        private PlayerMetrics previousPlayerMetrics = new PlayerMetrics();
        private PlayerMetrics previousEnemyMetrics = new PlayerMetrics();

        public PlayerAgent(String name, PlayerProperties playerProperties, ISkill[] skills)
        {
            // initialize player
            this.player = new Player(name, playerProperties, skills);
        }

        public virtual void Start()
        {
            // set name
            this.player.SetName(tag);
            // get rigit body
            this.agentRB = GetComponent<Rigidbody>();
            // initialize enemy
            BattleArena battleArena = GetComponentInParent<BattleArena>();
            this.enemy = battleArena.GetRival(tag).GetPLayer();
            this.rewardText = battleArena.GetRewardText(tag);
        }

        public IPlayer GetPLayer()
        {
            return this.player;
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.position = position;
        }

        public virtual void FixedUpdate()
        {
            // update position
            player.SetPosition(transform.position);
            // update timers
            player.UpdatePlayer(Time.fixedDeltaTime);
        }

        public override void OnEpisodeBegin()
        {
            player.ResetPlayer();
            // initialize position
            this.player.SetPosition(transform.position);
            // initialize player rewards
            this.rewards = new PlayerRewards(this.player, maxStep);
        }

        public override void OnActionReceived(float[] vectorAction)
        {
            // Debug.Log("Agent Move Action1 " + vectorAction[0]);
            // Debug.Log("Agent Skill Action2 " + vectorAction[1]);
            // Debug.Log("Agent Potion Action2 " + vectorAction[2]);

            // Save current metrics before any action
            SaveCurrentMetrics();

            // Make move actions
            MoveAction(Mathf.FloorToInt(vectorAction[0]));

            // Make skill and attack actions
            SkillAction(Mathf.FloorToInt(vectorAction[1]), enemy);

            // Make potion actions
            PotionAction(Mathf.FloorToInt(vectorAction[2]));

            // Give suitable rewards for this state
            GiveRewards();
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

            // Health information
            sensor.AddObservation(player.GetHealth());

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
            // player win
            if (enemy.IsDead())
            {
                SetReward(rewards.GetWinReward());
                EndEpisode();
            }
            // player loose
            else if (player.IsDead())
            {
                AddReward(rewards.GetLoseReward());
                EndEpisode();
            }
            else
            {
                // Give positive reward for using skills
                int usedSkillIndex = previousPlayerMetrics.GetUsedSkillIndex(player);
                if (usedSkillIndex >= 0)
                {
                    AddReward(rewards.GetSkillReward(usedSkillIndex));
                }

                // Give positive reward for debuff to enemy
                if (previousEnemyMetrics.DiffDebuffCount(enemy) != 0)
                {
                    AddReward(rewards.GetDebuffReward());
                }

                // Give positive reward for attacking to enemy
                if (previousPlayerMetrics.IsAttacked(player))
                {
                    AddReward(rewards.GetAttackReward());
                }

                // Give positive reward for using health pot according to need
                if (previousPlayerMetrics.DiffHealthPotionCount(player) != 0)
                {
                    AddReward(rewards.GetHealthUsageReward(player.GetHealth()));
                }

                // Give positive reward for using mana pot according to need
                if (previousPlayerMetrics.DiffManaPotionCount(player) != 0)
                {
                    AddReward(rewards.GetManaUsageReward(player.GetMana()));
                }

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

            // zero means no move
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
            // skill count + 1, means normal attack
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