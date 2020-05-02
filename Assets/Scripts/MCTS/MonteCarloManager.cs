using System;
using System.Threading.Tasks;

namespace PAPIOnline
{

	public class MonteCarloManager
	{
		public float fixedDeltaTime = 0.4f;
		public float searchTimeout = 0.15f;
		public int maxDepth = 50;
		private Action<float> giveMCTSReward;
		private Game game;
		private MonteCarlo mcts;
		private float[] vectorAction;

		public MonteCarloManager(IPlayer player, IPlayer enemy, Action<float> giveMCTSReward)
		{
			this.game = new Game(player, enemy, fixedDeltaTime);
			this.giveMCTSReward = giveMCTSReward;
		}

		public void GetActionReward(IPlayer player, IPlayer enemy, float[] vectorAction)
		{
			this.game.Reset(player, enemy);
			this.vectorAction = vectorAction;
			this.mcts = new MonteCarlo(game, maxDepth, MCTSFinishedForAction);

			Task.Run(() => mcts.RunSearch());
			Task.Run(() => mcts.EndSearch());
		}

		public void MCTSFinishedForAction()
		{
			float mctsReward = 0;

			GameState initialState = game.GetInitialState();
			int bestAction = mcts.BestAction(initialState);

			// calculate reward here
			if (bestAction != -1)
			{
				bool insideSelected = game.ActionInsideSelected(vectorAction, bestAction);
				// set reward here
			}
			
			UnityEngine.Debug.LogError("Best Action: " + bestAction);

			giveMCTSReward(mctsReward);
		}

		public void GetWinRateReward(IPlayer player, IPlayer enemy)
		{
			this.game.Reset(player, enemy);
			this.mcts = new MonteCarlo(game, maxDepth, MCTSFinishedForWinRate);

			Task.Run(() => mcts.RunSearch());
			Task.Run(() => mcts.EndSearch());
		}

		public void MCTSFinishedForWinRate()
		{
			float mctsReward = 0;

			GameState initialState = game.GetInitialState();
			MonteCarloResult result = mcts.GetResult(initialState);

			UnityEngine.Debug.LogError("plays: " + result.GetPlayCount() + " wins: " + result.GetWinCount());

			// set reward here

			giveMCTSReward(mctsReward);
		}

	}

}
