/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          GameHelper
 *   
 *   Description:    Helper class uses one action list for no memory allocation
					 and responsible for filling and making this actions
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;

namespace PAPIOnline
{

	public class GameHelper
	{
		public int ATTACK_INDEX;
		public int MOVE_INDEX;
		public int HEALTH_POTION_INDEX;
		public int MANA_POTION_INDEX;
		public bool[] allActions;

		public GameHelper(IPlayer player)
		{
			// All plays are consist of skills, attack, move and use potions
			this.allActions = new bool[player.GetSkillCount() + 1 + 1 + 2];
			this.ATTACK_INDEX = player.GetSkillCount();
			this.MOVE_INDEX = player.GetSkillCount() + 1;
			this.HEALTH_POTION_INDEX = player.GetSkillCount() + 2;
			this.MANA_POTION_INDEX = player.GetSkillCount() + 3;
		}

		public int FillAvailableActions(IPlayer player, IPlayer enemy)
		{
			// Fill skill masks
			int availableCount = 0;
			ISkill[] skills = player.GetSkills();
			float distance = Utils.GetDistance(player, enemy);
			for (int i = 0; i < skills.Length; i++)
			{
				ISkill skill = skills[i];
				// Check skill is available
				// Check player has enough mana to use skill
				// Check player close enough to use attack skill
				if (!player.IsAvailable() || !skill.IsAvailable() || player.GetMana() < skill.GetManaConsumption()
					|| (skill.GetSkillKind() == SkillKind.ATTACK && distance > ((IAttackSkill)skill).GetRange()))
				{
					allActions[i] = false;
				}
				else
				{
					allActions[i] = true;
					availableCount++;
				}
			}

			// Add attack mask after skill indexes
			if (!player.IsAvailable() || distance > player.GetAttackRange())
			{
				allActions[ATTACK_INDEX] = false;
			}
			else
			{
				allActions[ATTACK_INDEX] = true;
				availableCount++;
			}

			// Eliminate move actions, disable move if close enough to attack
			if (!player.IsAvailable() || availableCount > 0)
			{
				allActions[MOVE_INDEX] = false;
			}
			else
			{
				allActions[MOVE_INDEX] = true;
				availableCount++;
			}

			// Eliminate potion actions
			// Check player health is full
			// Check player has enough health potion
			if (player.GetHealthPotionCount() == 0 || player.GetHealthCapacity() == player.GetHealth())
			{
				allActions[HEALTH_POTION_INDEX] = false;
			}
			else
			{
				allActions[HEALTH_POTION_INDEX] = true;
				availableCount++;
			}

			// Check player mana is full
			// Check player has enough mana potion
			if (player.GetManaPotionCount() == 0 || player.GetManaCapacity() == player.GetMana())
			{
				allActions[MANA_POTION_INDEX] = false;
			}
			else
			{
				allActions[MANA_POTION_INDEX] = true;
				availableCount++;
			}

			return availableCount;
		}

		public void MakeAction(IPlayer player, IPlayer enemy, int action)
		{
			if (action == -1)
			{
				// There is not available action to do return
				return;
			}

			// Use skill
			if (action >= 0 && action < player.GetSkillCount())
			{
				player.UseSkill(action, enemy);
			}
			// Normal attack
			else if (action == ATTACK_INDEX)
			{
				player.Attack(enemy);
			}
			// Move
			else if (action == MOVE_INDEX)
			{
				Vector3 enemyDirection = Utils.GetDirection(player, enemy);
				float speed = Utils.GetDistance(player, enemy) / 2;
				speed /= player.GetSpeed();
				// Multiply direction to move immediately
				player.Move(enemyDirection * speed);
			}
			// Use health potion
			else if (action == HEALTH_POTION_INDEX)
			{
				player.UseHealthPotion();
			}
			// Use mana potion
			else if (action == MANA_POTION_INDEX)
			{
				player.UseManaPotion();
			}
			else
			{
				Debug.LogError("MCTS::Game unrecognised action came: " + action);
			}
		}

	}
}