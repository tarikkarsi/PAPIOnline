using UnityEngine;

namespace PAPIOnline
{

	public class PlayerRewards
	{
		private static readonly float ATTACK_SKILL_PERCENTAGE = 0.33f;
		private static readonly float BUFF_SKILL_PERCENTAGE = 0.17f;
		private static readonly float DEBUFF_SKILL_PERCENTAGE = 0.14f;
		private static readonly float ATTACK_PERCENTAGE = 0.16f;
		private static readonly float HEALTH_USAGE_PERCENTAGE = 0.15f;
		private static readonly float MANA_USAGE_PERCENTAGE = 0.5f;

		private static readonly float ATTACK_SKILL_COUNT = 30;
		private static readonly float BUFF_SKILL_COUNT = 2;
		private static readonly float DEBUFF_SKILL_COUNT = 8;
		private static readonly float ATTACK_COUNT = 100;
		private static readonly float HEALTH_USAGE_COUNT = 10;
		private static readonly float MANA_USAGE_COUNT = 10;

		private static readonly float WIN_REWARD = 1;
		private static readonly float LOSE_REWARD = -1;
		private static readonly float STEP_REWARD = -1;
		private static readonly float ATTACK_SKILL_REWARD = ATTACK_SKILL_PERCENTAGE / ATTACK_SKILL_COUNT;
		private static readonly float BUFF_SKILL_REWARD = BUFF_SKILL_PERCENTAGE / BUFF_SKILL_COUNT;
		private static readonly float DEBUFF_SKILL_REWARD = DEBUFF_SKILL_PERCENTAGE / DEBUFF_SKILL_COUNT;
		private static readonly float ATTACK_REWARD = ATTACK_PERCENTAGE / ATTACK_COUNT;
		private static readonly float HEALTH_USAGE_REWARD = HEALTH_USAGE_PERCENTAGE / HEALTH_USAGE_COUNT;
		private static readonly float MANA_USAGE_REWARD = MANA_USAGE_PERCENTAGE / MANA_USAGE_COUNT;

		private string playerName;
		private float winReward;
		private float loseReward;
		private float stepReward;
		private float[] skillRewards;
		private float debuffReward;
		private float attackReward;
		private float healthUsageReward;
		private float manaUsageReward;

		private int healthCapacity;
		private int manaCapacity;

		public PlayerRewards(IPlayer player, int maxStep)
		{
			ISkill[] skills = player.GetSkills();
			playerName = player.GetName();
			winReward = WIN_REWARD;
			loseReward = LOSE_REWARD;
			stepReward = STEP_REWARD / maxStep;
			debuffReward = DEBUFF_SKILL_REWARD;
			attackReward = ATTACK_REWARD;
			healthUsageReward = HEALTH_USAGE_REWARD;
			manaUsageReward = MANA_USAGE_REWARD;
			SetSkillRewards(player.GetSkills());

			this.healthCapacity = player.GetHealthCapacity();
			this.manaCapacity = player.GetManaCapacity();
		}


		private void SetSkillRewards(ISkill[] skills)
		{
			float maxDamage = GetMaxDamage(skills);
			skillRewards = new float[skills.Length];
			for (int i = 0; i < skills.Length; i++)
			{
				ISkill skill = skills[i];
				if (skill.GetSkillKind() == SkillKind.ATTACK)
				{
					skillRewards[i] = GetAttackSkillWeightedReward(((IAttackSkill)skill), maxDamage);
				}
				else if (skill.GetSkillKind() == SkillKind.BUFF)
				{
					skillRewards[i] = BUFF_SKILL_REWARD;
				}
				else if (skill.GetSkillKind() == SkillKind.DEBUFF)
				{
					// debuff skill rewards given under the debuff effect
					// so do not need to give seperate reward here
					skillRewards[i] = 0;
				}
			}
		}

		private static float GetAttackSkillWeightedReward(IAttackSkill attackSKill, float maxDamage)
		{
			return ATTACK_SKILL_REWARD * (1 + attackSKill.GetDamage() / maxDamage);
		}

		private static float GetMaxDamage(ISkill[] skills)
		{
			float maxDamage = 0;
			foreach (ISkill skill in skills)
			{
				if (skill.GetSkillKind() == SkillKind.ATTACK)
				{
					float damage = ((IAttackSkill)skill).GetDamage();
					if (damage > maxDamage)
					{
						maxDamage = damage;
					}
				}
			}
			return maxDamage;
		}

		public float GetWinReward()
		{
			return this.winReward;
		}

		public float GetLoseReward()
		{
			return this.loseReward;
		}

		public float GetStepReward()
		{
			return this.stepReward;
		}

		public float GetSkillReward(int skillIndex)
		{
			return this.skillRewards[skillIndex];
		}

		public float GetDebuffReward()
		{
			return this.debuffReward;
		}

		public float GetAttackReward()
		{
			return this.attackReward;
		}

		public float GetHealthUsageReward(int currentHealth)
		{
			float decreasedHealth = healthCapacity - currentHealth;
			// give less reward unnecessary health usage
			if (decreasedHealth > PlayerProperties.HEALTH_POTION_FILL)
			{
				return this.healthUsageReward;
			}
			else
			{
				return this.healthUsageReward * (decreasedHealth / PlayerProperties.HEALTH_POTION_FILL);
			}
		}

		public float GetManaUsageReward(int currentMana)
		{
			float decreasedMana = manaCapacity - currentMana;
			// give less reward unnecessary health usage
			if (decreasedMana > PlayerProperties.MANA_POTION_FILL)
			{
				return this.manaUsageReward;
			}
			else
			{
				return this.manaUsageReward * (decreasedMana / PlayerProperties.MANA_POTION_FILL);
			}
		}

		public void Print()
		{
			Debug.Log("*****************************************************");
			Debug.Log(playerName + " rewards:");
			Debug.Log("winReward = " + winReward);
			Debug.Log("loseReward = " + loseReward);
			Debug.Log("stepReward = " + stepReward);
			for (int i = 0; i < skillRewards.Length; i++)
			{
				Debug.Log("skill[" + i + "]Reward = " + skillRewards[i]);
			}
			Debug.Log("debuffReward = " + debuffReward);
			Debug.Log("attackReward = " + attackReward);
			Debug.Log("healthUsageReward = " + healthUsageReward);
			Debug.Log("manaUsageReward = " + manaUsageReward);
			Debug.Log("*****************************************************");
		}
	}

}