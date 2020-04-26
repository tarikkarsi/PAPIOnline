using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PAPIOnline
{

	public class MonteCarloManager
	{
		public float fixedDeltaTime = 0.4f;
		public float searchTimeout = 0.15f;
		public int maxDepth = 50;
		private bool[] playerAllActions;
		private bool[] enemyAllActions;

		public MonteCarloManager(IPlayer player, IPlayer enemy)
		{
			// all plays are consist of skills, attack, move and use potions
			this.playerAllActions = new bool[player.GetSkillCount() + 1 + 1 + 2];
			this.enemyAllActions = new bool[enemy.GetSkillCount() + 1 + 1 + 2];
		}

		public void GetActionReward(IPlayer player, IPlayer enemy, float[] vectorAction, Action mctsFinished)
		{
			Game game = new Game(player, enemy, ref this.playerAllActions, ref this.enemyAllActions, fixedDeltaTime);
			MonteCarlo mcts = new MonteCarlo(game, maxDepth, mctsFinished);

			Task.Run(() => mcts.RunSearch());
			Task.Run(() => mcts.EndSearch());
		}

		public void GetActionReward2(IPlayer player, IPlayer enemy, float[] vectorAction, Action mctsFinished)
		{
			Game game = new Game(player, enemy, ref this.playerAllActions, ref this.enemyAllActions, fixedDeltaTime);
			MonteCarlo mcts = new MonteCarlo(game, maxDepth, mctsFinished);

			Thread t = new Thread(mcts.RunSearch);
			t.Name = "PAPIOnlineMCTSThread";
			t.Priority = System.Threading.ThreadPriority.Highest;
			t.Start();
			Task.Run(() => mcts.EndSearch());
		}

		public void GetWinRateReward(IPlayer player, IPlayer enemy)
		{
			
		}

		

	}

}
