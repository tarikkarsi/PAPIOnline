using System;

public class MonteCarloRunner
{
	public static float FIXED_DELTA_TIME = 0.2f;
	private float timeout;
	private bool initialized = false;
	private int[] playerAllActions;
	private int[] enemyAllActions;

	public void Initialize(IPlayer player, IPlayer enemy, float timeout)
	{
		if (!this.initialized)
		{
			this.timeout = timeout;
			// all plays are consist of skills, attack and move
			this.playerAllActions = new int[player.GetSkillCount() + 1 + 1];
			for (int i = 0; i < playerAllActions.Length; i++)
			{
				playerAllActions[i] = i;
			}
			this.enemyAllActions = new int[enemy.GetSkillCount() + 1 + 1];
			for(int i = 0; i < enemyAllActions.Length; i++)
			{
				enemyAllActions[i] = i;
			}
			this.initialized = true;
		}
	}

	public void Run(IPlayer player, IPlayer enemy)
	{
		Game game = new Game(player, enemy, this.playerAllActions, this.enemyAllActions, FIXED_DELTA_TIME);
		MonteCarlo mcts = new MonteCarlo(game);
		GameState state = game.Start();
		PlayerKind winner = game.Winner(state);
		// From initial state, take turns to play game until someone wins
		while (winner == PlayerKind.NONE)
		{
			mcts.RunSearch(state, timeout);
			int action = mcts.BestAction(state);
			state = game.NextState(state, action);
			winner = game.Winner(state);
		}

		UnityEngine.Debug.Log("Winner " + winner);
	}
}
