/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          MonteCarloManager
 *   
 *   Description:    Manager that controls MCTS running time and calculates
 *					 rewards according to the result
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PAPIOnline
{

	public enum MonterCarloRewardType
	{
		WIN_RATE,
		ACTION,
	};

	public class MonteCarloManager
	{
		public float fixedDeltaTime = 0.4f;
		public float searchTimeout = 0.15f;
		public int maxDepth = 50;
		private Action<float> giveMCTSReward;
		private MonteCarlo mcts;
		private float[] vectorAction;
		private MonterCarloRewardType rewardType;

		public MonteCarloManager(IPlayer player, IPlayer enemy, Action<float> giveMCTSReward, MonterCarloRewardType rewardType)
		{
			this.mcts = new MonteCarlo(new Game(player, enemy, fixedDeltaTime), maxDepth, MCTSFinished);
			this.giveMCTSReward = giveMCTSReward;
			this.rewardType = rewardType;
		}

		public void GetReward(IPlayer player, IPlayer enemy, float[] vectorAction)
		{
			// Reset the MCTS
			this.mcts.Reset(player, enemy);
			this.vectorAction = vectorAction;
			Task.Run(() => mcts.RunSearch());
			Task.Run(() => mcts.EndSearch());
		}

		public void MCTSFinished()
		{
			if (rewardType == MonterCarloRewardType.ACTION)
			{
				GiveMCTSActionReward();
			}
			else
			{
				GiveMCTSWinRateReward();
			}
		}

		public void GiveMCTSActionReward()
		{
			float mctsReward = 0;
			MonteCarloNode rootNode = mcts.GetRootNode();
			List<int> mctsActions = rootNode.AllActions();
			ISet<int> annActions = mcts.ConvertMCTSActions(this.vectorAction);

			double bestMCTSRate = -1;
			double bestANNRate = -1;
			foreach (int action in mctsActions)
			{
				MonteCarloNode childNode = rootNode.ChildNode(action);
				if (childNode != null)
				{
					double ratio = ((double)childNode.numberOfWins) / childNode.numberOfPlays;
					// Set MCTS action max ratio
					if (ratio > bestMCTSRate)
					{
						bestMCTSRate = ratio;
					}
					// Set ANN action max ratio
					if (annActions.Contains(action) && ratio > bestANNRate)
					{
						bestANNRate = ratio;
					}
				}
			}

			//move action rewards discarded
			if (bestANNRate != -1)
			{
				double normalizedANNRate = bestANNRate / bestMCTSRate;
				double differenceFromMax = 1 - normalizedANNRate;
				double diffSquare = Math.Pow(differenceFromMax, 2);
				mctsReward = (float)(1.3d * Math.Exp(-5.84d * diffSquare) - 0.3d);
			}

			giveMCTSReward(mctsReward);
		}

		public void GiveMCTSWinRateReward()
		{
			MonteCarloNode result = mcts.GetRootNode();

			UnityEngine.Debug.LogError("plays: " + result.numberOfWins + " wins: " + result.numberOfPlays);

			// Calculate reward, for %0 = -0.3, %50 = 0, %100 = 1

			float winRate = result.numberOfWins / result.numberOfPlays;
			float mctsReward = (float)(Math.Exp(2.4d * winRate) - 0.42d);

			giveMCTSReward(mctsReward);
		}



	}

}
