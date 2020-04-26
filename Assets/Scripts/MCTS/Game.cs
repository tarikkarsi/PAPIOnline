using System.Collections.Generic;
using UnityEngine;

namespace PAPIOnline
{

	public class Game
	{
		private GameState initialState;
		private bool[] playerAllActions;
		private bool[] enemyAllActions;
		private float fixedDeltaTime;
		public int ATTACK_INDEX;
		public int MOVE_INDEX;
		public int HEALTH_POTION_INDEX;
		public int MANA_POTION_INDEX;

		private static readonly System.Random random = new System.Random();

		public static int RandomNumber(int min, int max)
		{
			return random.Next(min, max);
		}

		public Game(IPlayer player, IPlayer enemy, ref bool[] playerAllActions, ref bool[] enemyAllActions, float fixedDeltaTime)
		{
			// initial state owner is player
			this.initialState = new GameState(player, enemy, PlayerKind.PLAYER);
			this.playerAllActions = playerAllActions;
			this.enemyAllActions = enemyAllActions;
			this.fixedDeltaTime = fixedDeltaTime;

			this.ATTACK_INDEX = player.GetSkillCount();
			this.MOVE_INDEX = player.GetSkillCount() + 1;
			this.HEALTH_POTION_INDEX = player.GetSkillCount() + 2;
			this.MANA_POTION_INDEX = player.GetSkillCount() + 3;
		}

		/** Generate and return the initial game state. */
		public GameState Start()
		{
			return this.initialState;
		}

		/** Return the current player’s legal moves from given state. */
		public bool[] LegalActions(GameState state)
		{
			if (state.GetTurn() == PlayerKind.PLAYER)
			{
				FillAvailableActions(state.GetPlayer(), state.GetEnemy(), ref this.playerAllActions);
				return this.playerAllActions;
			}
			else
			{
				FillAvailableActions(state.GetEnemy(), state.GetPlayer(), ref this.enemyAllActions);
				return this.enemyAllActions;
			}
		}

		public int RandomLegalAction(IPlayer player, IPlayer enemy, PlayerKind turn)
		{
			int legalActionCount = 0;
			bool[] allActions;
			if (turn == PlayerKind.PLAYER)
			{
				legalActionCount = FillAvailableActions(player, enemy, ref this.playerAllActions);
				allActions = this.playerAllActions;
			}
			else
			{
				legalActionCount = FillAvailableActions(enemy, player, ref this.enemyAllActions);
				allActions = this.enemyAllActions;
			}

			return RandomAction(allActions, legalActionCount);
		}

		public int RandomAction(bool[] allActions, int legalActionCount)
		{
			int action = -1;
			if (legalActionCount > 0)
			{
				int rn = RandomNumber(0, legalActionCount);
				for (int i = 0; i < allActions.Length; i++)
				{
					if (allActions[i])
					{
						if (rn == 0)
						{
							action = i;
							break;
						}
						rn--;
					}
				}
			}

			return action;
		}

		public int RandomAction(List<int> actions)
		{
			if (actions.Count > 0)
			{
				int rn = RandomNumber(0, actions.Count);
				return actions[rn];
			}

			return -1;
		}

		/** Advance the given state and return it. */
		public GameState NextState(GameState state, int play)
		{
			// clone player and enemy to new state
			IPlayer newPlayer = state.GetPlayer().ClonePlayer();
			IPlayer newEnemy = state.GetEnemy().ClonePlayer();

			// make action
			PlayerKind newTurn = MakeAction(newPlayer, newEnemy, play, state.GetTurn());
			
			// update players
			UpdatePlayers(newPlayer, newEnemy);

			// create new state with new player and enemy
			return new GameState(newPlayer, newEnemy, newTurn);
		}

		public PlayerKind MakeAction(IPlayer player, IPlayer enemy, int play, PlayerKind turn)
		{
			// make action
			if (turn == PlayerKind.PLAYER)
			{
				MakeAction(player, enemy, play);
			}
			else
			{
				MakeAction(enemy, player, play);
			}

			return ChangeTurn(turn);
		}

		public void UpdatePlayers(IPlayer player, IPlayer enemy)
		{
			player.UpdatePlayer(fixedDeltaTime);
			enemy.UpdatePlayer(fixedDeltaTime);
		}

		/** Return the winner of the game. */
		public PlayerKind Winner(GameState state)
		{
			return Winner(state.GetPlayer(), state.GetEnemy());
		}

		public PlayerKind Winner(IPlayer player, IPlayer enemy)
		{
			return player.IsDead() ? PlayerKind.ENEMY : enemy.IsDead() ? PlayerKind.PLAYER : PlayerKind.NONE;
		}

		public int FillAvailableActions(IPlayer player, IPlayer enemy, ref bool[] allActions)
		{
			// fill skill masks
			int availableCount = 0;
			ISkill[] skills = player.GetSkills();
			float distance = Utils.GetDistance(player, enemy);
			for (int i = 0; i < skills.Length; i++)
			{
				ISkill skill = skills[i];
				// check skill is available
				// check player has enough mana to use skill
				// check player close enough to use attack skill
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

			// add attack mask after skill indexes
			if (!player.IsAvailable() || distance > player.GetAttackRange())
			{
				allActions[ATTACK_INDEX] = false;
			}
			else
			{
				allActions[ATTACK_INDEX] = true;
				availableCount++;
			}
			
			// eliminate move actions, disable move if close enough to attack
			if (!player.IsAvailable() || availableCount > 0)
			{
				allActions[MOVE_INDEX] = false;
			}
			else
			{
				allActions[MOVE_INDEX] = true;
				availableCount++;
			}

			// eliminate potion actions
			List<int> potionMasks = new List<int>();
			// check player health is full enough
			// check player has enough health potion
			if (player.GetHealthCapacity() - player.GetHealth() < PlayerProperties.HEALTH_POTION_FILL
				|| player.GetHealthPotionCount() == 0)
			{
				allActions[HEALTH_POTION_INDEX] = false;
			}
			else
			{
				allActions[HEALTH_POTION_INDEX] = true;
				availableCount++;
			}

			// check player mana is full
			// check player has enough mana potion
			if (player.GetManaCapacity() - player.GetMana() < PlayerProperties.MANA_POTION_FILL
				|| player.GetManaPotionCount() == 0)
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

		private void MakeAction(IPlayer player, IPlayer enemy, int action)
		{
			if (action == -1)
			{
				// if there is not available action to do return
				return;
			}

			// use skill
			if (action >= 0 && action < player.GetSkillCount())
			{
				player.UseSkill(action, enemy);
			}
			// normal attack
			else if (action == ATTACK_INDEX)
			{
				player.Attack(enemy);
			}
			// move
			else if (action == MOVE_INDEX)
			{
				Vector3 enemyDirection = Utils.GetDirection(player, enemy);
				// multiply direction to move immediately
				player.Move(enemyDirection * 3.5f);
			}
			// use health potion
			else if (action == HEALTH_POTION_INDEX)
			{
				player.UseHealthPotion();
			}
			// use mana potion
			else if (action == MANA_POTION_INDEX)
			{
				player.UseManaPotion();
			}
			else
			{
				Debug.LogError("MCTS::Game unrecognised action came: " + action);
			}
		}

		private PlayerKind ChangeTurn(PlayerKind currentTurn)
		{
			return currentTurn == PlayerKind.PLAYER ? PlayerKind.ENEMY : PlayerKind.PLAYER;
		}
	}

}