using System.Collections.Generic;
using UnityEngine;

public class Game
{
	private GameState initialState;
	private int[] playerAllActions;
	private int[] enemyAllActions;
	private float fixedDeltaTime;
	
	public Game(IPlayer player, IPlayer enemy, int[] playerAllActions, int[] enemyAllActions, float fixedDeltaTime)
	{
		// initial state owner is player
		this.initialState = new GameState(player, enemy, PlayerKind.PLAYER);
		this.playerAllActions = playerAllActions;
		this.enemyAllActions = enemyAllActions;
		this.fixedDeltaTime = fixedDeltaTime;
	}

	/** Generate and return the initial game state. */
	public GameState Start()
	{	
		return this.initialState;
	}

	/** Return the current player’s legal moves from given state. */
	public int[] LegalActions(GameState state)
	{
		if (state.IsPlayer())
		{
			return FindAvailableActions(state.GetPlayer(), state.GetEnemy(), this.playerAllActions);
		}
		else
		{
			return FindAvailableActions(state.GetEnemy(), state.GetPlayer(), this.enemyAllActions);
		}		
	}

	/** Advance the given state and return it. */
	public GameState NextState(GameState state, int play)
	{
		// update player and enemy
		state.UpdateState(fixedDeltaTime);

		// clone player and enemy to new state
		IPlayer newPlayer = state.GetPlayer().ClonePlayer();
		IPlayer newEnemy = state.GetEnemy().ClonePlayer();
		// make action
		if (state.IsPlayer())
		{
			MakeAction(newPlayer, newEnemy, play);
		}
		else
		{
			MakeAction(newEnemy, newPlayer, play);
		}
		// create new state with new player and enemy
		return new GameState(newPlayer, newEnemy, ChangeTurn(state.GetPlayerKind()));
	}

	/** Return the winner of the game. */
	public PlayerKind Winner(GameState state)
	{
		if (state.GetPlayer().IsDead())
		{
			return PlayerKind.ENEMY;
		}
		else if (state.GetEnemy().IsDead())
		{
			return PlayerKind.PLAYER;
		}

		// TODO
		// check specified depth reached

		return PlayerKind.NONE;
	}

	private int[] FindAvailableActions(IPlayer player, IPlayer enemy, int[] allActions)
	{
		List<int> availableActions = new List<int>(allActions);
		int[] skillMasks = Utils.GetSkillMasks(player, enemy);
		foreach (int skillMask in skillMasks)
		{
			availableActions.Remove(skillMask);
		}
		int[] moveMasks = Utils.GetMoveMasks(player);
		if (moveMasks.Length > 0)
		{
			// last index is move action
			availableActions.Remove(availableActions.Count - 1);
		}
		return availableActions.ToArray();
	}

	private void MakeAction(IPlayer player, IPlayer enemy, int action)
	{
		int skillCount = player.GetSkillCount();
		// zero means idle
		// 0 - skill count - 1, means skill using skill
		if (action >= 0 && action < skillCount)
		{
			player.UseSkill(action, enemy);
		}
		// skill count, means normal attack
		else if (action == skillCount)
		{
			player.Attack(enemy);
		}
		// skill count + 1, means move
		else if (action == skillCount)
		{
			Vector3 enemyDirection = Utils.GetDirection(player.GetPosition(), enemy.GetPosition());
			player.Move(enemyDirection);
		}
		// otherwise do nothing
	}

	private PlayerKind ChangeTurn(PlayerKind currentTurn)
	{
		return currentTurn == PlayerKind.PLAYER ? PlayerKind.ENEMY : PlayerKind.PLAYER;
	}
}
