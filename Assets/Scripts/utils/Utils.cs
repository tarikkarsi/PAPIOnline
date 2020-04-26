using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public static class Utils
	{

		public static float GetDistance(IPlayer from, IPlayer to)
		{
			return Vector3.Distance(from.GetPosition(), to.GetPosition());
		}

		public static Vector3 GetDirection(IPlayer from, IPlayer to)
		{
			return (to.GetPosition() - from.GetPosition()).normalized;
		}

		public static bool CanAttack(IPlayer from, IPlayer to)
		{
			return GetDistance(from, to) <= from.GetAttackRange();
		}

		/*
		 * This method returns array of indices of not available movements on the next iteration
		 * Checks character stunned or not, if stunned no movement will be available
		 */
		public static int[] GetMoveMasks(IPlayer player)
		{
			// player can not move when stunned
			// starts from 1 because of zero is idle
			return !player.IsAvailable() ? new int[] { 1, 2, 3, 4 } : new int[0];
		}

		/*
		 * This method returns array of indices of not available skills on the next iteration
		 * Checks characher stunned or not, if stunned no skill can be used
		 * Checks distance for attack skills, if not close enough, skill disabled
		 */
		public static int[] GetSkillMasks(IPlayer player, IPlayer enemy)
		{
			// player can not use any skills and attack when stunned, dead or animating
			if (!player.IsAvailable())
			{
				return new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			}

			List<int> skillMasks = new List<int>();
			// add skill masks
			int i = 0;
			ISkill[] skills = player.GetSkills();
			float distance = Utils.GetDistance(player, enemy);
			for (; i < skills.Length; i++)
			{
				ISkill skill = skills[i];
				// check skill is available
				// check player has enough mana to use skill
				// check player close enough to use attack skill
				if (!skill.IsAvailable() || player.GetMana() < skill.GetManaConsumption() || (skill.GetSkillKind() == SkillKind.ATTACK &&
					distance > ((IAttackSkill)skill).GetRange()))
				{
					// starts from 1 because of zero is idle
					skillMasks.Add(i + 1);
				}
			}

			// add attack mask after skill indexes
			if (distance > player.GetAttackRange())
			{
				skillMasks.Add(i + 1);
			}

			return skillMasks.ToArray();
		}

		public static int[] GetPotionMasks(IPlayer player)
		{
			List<int> potionMasks = new List<int>();
			// check player health is full
			// check player has enough health potion
			if (player.GetHealth() == player.GetHealthCapacity() || player.GetHealthPotionCount() == 0)
			{
				potionMasks.Add(1);
			}
			// check player mana is full
			// check player has enough mana potion
			if (player.GetMana() == player.GetManaCapacity() || player.GetManaPotionCount() == 0)
			{
				potionMasks.Add(2);
			}

			return potionMasks.ToArray();
		}

	}

}