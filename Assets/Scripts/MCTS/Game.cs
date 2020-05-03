/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          Game
 *   
 *   Description:    Game class which controls actions and states
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System.Collections.Generic;

namespace PAPIOnline
{

	public class Game
	{
		private GameState initialState;
		private float fixedDeltaTime;
		private GameHelper playerHelper;
		private GameHelper enemyHelper;

		private static readonly System.Random random = new System.Random();

		public static int RandomNumber(int min, int max)
		{
			return random.Next(min, max);
		}

		public Game(IPlayer player, IPlayer enemy, float fixedDeltaTime)
		{
			this.playerHelper = new GameHelper(player);
			this.enemyHelper = new GameHelper(enemy);
			this.fixedDeltaTime = fixedDeltaTime;
		}

		public void Reset(IPlayer player, IPlayer enemy)
		{
			// Initial state owner is player
			this.initialState = new GameState(player, enemy, PlayerKind.PLAYER);
		}

		public GameState GetInitialState()
		{
			return this.initialState;
		}

		public bool[] LegalActions(GameState state)
		{
			if (state.GetTurn() == PlayerKind.PLAYER)
			{
				playerHelper.FillAvailableActions(state.GetPlayer(), state.GetEnemy());
				return playerHelper.allActions;
			}
			else
			{
				enemyHelper.FillAvailableActions(state.GetEnemy(), state.GetPlayer());
				return enemyHelper.allActions;
			}
		}

		public int RandomLegalAction(IPlayer player, IPlayer enemy, PlayerKind turn)
		{
			int legalActionCount;
			if (turn == PlayerKind.PLAYER)
			{
				legalActionCount = playerHelper.FillAvailableActions(player, enemy);
				return RandomAction(playerHelper.allActions, legalActionCount);
			}
			else
			{
				legalActionCount = enemyHelper.FillAvailableActions(enemy, player);
				return RandomAction(enemyHelper.allActions, legalActionCount);
			}
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

		public GameState NextState(GameState state, int play)
		{
			// Clone player and enemy to new state
			IPlayer newPlayer = state.GetPlayer().ClonePlayer();
			IPlayer newEnemy = state.GetEnemy().ClonePlayer();

			// Make action
			PlayerKind newTurn = MakeAction(newPlayer, newEnemy, play, state.GetTurn());
			
			// Update players
			UpdatePlayers(newPlayer, newEnemy);

			// Create new state with new player and enemy
			return new GameState(newPlayer, newEnemy, newTurn);
		}

		public PlayerKind MakeAction(IPlayer player, IPlayer enemy, int play, PlayerKind turn)
		{
			// Make action
			if (turn == PlayerKind.PLAYER)
			{
				playerHelper.MakeAction(player, enemy, play);
			}
			else
			{
				enemyHelper.MakeAction(enemy, player, play);
			}

			return ChangeTurn(turn);
		}

		public void UpdatePlayers(IPlayer player, IPlayer enemy)
		{
			player.UpdatePlayer(fixedDeltaTime);
			enemy.UpdatePlayer(fixedDeltaTime);
		}

		public PlayerKind Winner(GameState state)
		{
			return Winner(state.GetPlayer(), state.GetEnemy());
		}

		public PlayerKind Winner(IPlayer player, IPlayer enemy)
		{
			return player.IsDead() ? PlayerKind.ENEMY : enemy.IsDead() ? PlayerKind.PLAYER : PlayerKind.NONE;
		}

		private PlayerKind ChangeTurn(PlayerKind currentTurn)
		{
			return currentTurn == PlayerKind.PLAYER ? PlayerKind.ENEMY : PlayerKind.PLAYER;
		}

		public bool ActionInsideSelected(float[] vectorAction, int action)
		{
			// Skill action
			if (action <= playerHelper.ATTACK_INDEX)
			{
				float skillAction = vectorAction[PlayerAgent.SKILL_BRANCH_INDEX];
				return skillAction - 1 == action;
			}
			// Move action
			else if (action == playerHelper.MOVE_INDEX)
			{
				return vectorAction[PlayerAgent.MOVE_BRANCH_INDEX] != 0;
			}
			// Health use action
			else if (action == playerHelper.HEALTH_POTION_INDEX)
			{
				return vectorAction[PlayerAgent.POTION_BRANCH_INDEX] == 1;
			}
			// Mana use action
			else if (action == playerHelper.MANA_POTION_INDEX)
			{
				return vectorAction[PlayerAgent.POTION_BRANCH_INDEX] == 2;
			}
			else
			{
				UnityEngine.Debug.LogError("Game::ActionInsideSelected unrecognised action came " + action);
			}
			return false;
		}
	}

}