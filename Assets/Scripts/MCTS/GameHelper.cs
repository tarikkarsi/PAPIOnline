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
		public int SKILL_START_INDEX;
		public int ATTACK_INDEX;
		public int MOVE_INDEX;
		public int HEALTH_POTION_INDEX;
		public int MANA_POTION_INDEX;
		public bool[] allActions;

		public GameHelper(IPlayer player)
		{
			// All plays are consist of skills, attack, move and use potions
			this.allActions = new bool[player.GetSkillCount() + 1 + 1 + 2];
			this.SKILL_START_INDEX = 0;
			this.ATTACK_INDEX = this.SKILL_START_INDEX + player.GetSkillCount();
			this.MOVE_INDEX = this.ATTACK_INDEX + 1;
			this.HEALTH_POTION_INDEX = this.MOVE_INDEX + 1;
			this.MANA_POTION_INDEX = this.HEALTH_POTION_INDEX + 1;
		}

		public int FillAvailableActions(IPlayer player, IPlayer enemy)
		{
			int availableCount = 0;
			// Fill skill actions
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
					allActions[SKILL_START_INDEX + i] = false;
				}
				else
				{
					allActions[SKILL_START_INDEX + i] = true;
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

			// Fill move actions, disable move if close enough to attack
			if (!player.IsAvailable() || availableCount > 0)
			{
				allActions[MOVE_INDEX] = false;
			}
			else
			{
				allActions[MOVE_INDEX] = true;
				availableCount++;
			}

			// Fill potion actions
			// Check player health is full
			// Check player has enough health potion
			if (player.GetHealthPotionCount() == 0 || (player.GetHealthCapacity() - player.GetHealth() < PlayerProperties.HEALTH_POTION_FILL))
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
			if (player.GetManaPotionCount() == 0 || (player.GetManaCapacity() - player.GetMana() < PlayerProperties.MANA_POTION_FILL))
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
			if (action >= SKILL_START_INDEX && action < ATTACK_INDEX)
			{
				player.UseSkill(action - SKILL_START_INDEX, enemy);
			}
			// Melee attack
			else if (action == ATTACK_INDEX)
			{
				player.Attack(enemy);
			}
			// Move
			else if (action == MOVE_INDEX)
			{
				// Movement speed increased to make MCTS tree depth small  
				Vector3 enemyDirection = Utils.GetDirection(player, enemy);
				float speed = Utils.GetDistance(player, enemy) / 2;
				speed /= player.GetSpeed();
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
				Debug.LogError(player.GetName() + " unrecognised action came (GameHelper::MakeAction) action: " + action);
			}
		}

	}
}