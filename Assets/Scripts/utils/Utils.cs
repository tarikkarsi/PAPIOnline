/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          Utils
 *   
 *   Description:    Utility class for some calculations
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public class Utils
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
			// Player can not move when stunned
			// Starts from 1 because of zero is idle
			return !player.IsAvailable() ?  new int[] { 1, 2, 3, 4 } : new int[0];
		}

		/*
		 * This method returns array of indices of not available skills on the next iteration
		 * Checks characher stunned or not, if stunned no skill can be used
		 * Checks distance for attack skills, if not close enough, skill disabled
		 */
		public static int[] GetSkillMasks(IPlayer player, IPlayer enemy)
		{
			// Player can not use any skills and attack when stunned, dead or animating
			if (!player.IsAvailable())
			{
				return new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			}

			List<int> skillMasks = new List<int>();
			// Add skill masks
			int i = 0;
			ISkill[] skills = player.GetSkills();
			float distance = Utils.GetDistance(player, enemy);
			for (; i < skills.Length; i++)
			{
				ISkill skill = skills[i];
				// Check skill is available
				// Check player has enough mana to use skill
				// Check player close enough to use attack skill
				if (!skill.IsAvailable() || player.GetMana() < skill.GetManaConsumption() || (skill.GetSkillKind() == SkillKind.ATTACK &&
					distance > ((IAttackSkill)skill).GetRange()))
				{
					// Starts from 1 because of zero is idle
					skillMasks.Add(i + 1);
				}
			}

			// Add attack mask after skill indexes
			if (distance > player.GetAttackRange())
			{
				skillMasks.Add(i + 1);
			}

			return skillMasks.ToArray();
		}

		public static int[] GetPotionMasks(IPlayer player)
		{
			List<int> potionMasks = new List<int>();
			// Check player health is decreased enough
			// Check player has enough health potion
			if (player.GetHealthPotionCount() == 0 || player.GetHealthCapacity() == player.GetHealth())
			{
				potionMasks.Add(1);
			}
			// Check player mana is decreased enough
			// Check player has enough mana potion
			if (player.GetManaPotionCount() == 0 || player.GetManaCapacity() == player.GetMana())
			{
				potionMasks.Add(2);
			}

			return potionMasks.ToArray();
		}

	}

}