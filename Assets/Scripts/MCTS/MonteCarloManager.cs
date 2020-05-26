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
using System.Collections.Generic;

namespace PAPIOnline
{

	public class MonteCarloManager
	{
		public float fixedDeltaTime = 0.4f;
		public float searchTimeout = 0.15f;
		public int maxSimulation = 30;
		public int maxDepth = 50;
		public int UCB1ExploreParam = 2;
		private MonteCarlo mcts;

		public MonteCarloManager(IPlayer player, IPlayer enemy)
		{
			this.mcts = new MonteCarlo(new Game(player, enemy, fixedDeltaTime), maxSimulation, maxDepth, UCB1ExploreParam);
		}

		public float GetReward(IPlayer player, IPlayer enemy, float[] vectorAction)
		{
			// Reset the MCTS
			this.mcts.Reset(player, enemy);
			this.mcts.RunSearch();
			return GiveMCTSActionReward(vectorAction);
		}

		public float GiveMCTSActionReward(float[] vectorAction)
		{
			float mctsReward = 0;
			MonteCarloNode rootNode = mcts.GetRootNode();
			List<int> mctsActions = rootNode.AllActions();
			ISet<int> annActions = mcts.ConvertMCTSActions(vectorAction);

			double MCTSBestUCB = Double.NegativeInfinity;
			double ANNBestUCB = Double.NegativeInfinity;
			double UCBMin = Double.PositiveInfinity;
			double UCB = 0;
			// Find best UCB for MCTS and ANN actions
			foreach (int action in mctsActions)
			{
				MonteCarloNode childNode = rootNode.ChildNode(action);
				if (childNode != null)
				{
					UCB = childNode.GetUCB1(UCB1ExploreParam);
					// Set MCTS action max UCB
					if (UCB > MCTSBestUCB)
					{
						MCTSBestUCB = UCB;
					}
					// Set ANN action max UCB
					if (annActions.Contains(action) && UCB > ANNBestUCB)
					{
						ANNBestUCB = UCB;
					}
					// Set min UCB
					if (UCB < UCBMin)
					{
						UCBMin = UCB;
					}
				}
			}

			// Move action discarded
			if (ANNBestUCB != Double.NegativeInfinity)
			{
				// Prevent divide by zero assign too little values
				UCBMin = UCBMin == MCTSBestUCB ? 0 : UCBMin;
				MCTSBestUCB = MCTSBestUCB == 0 ? 000000000.1d : MCTSBestUCB;
				ANNBestUCB = ANNBestUCB == 0 ? 000000000.1d : ANNBestUCB;
				// Normalize the ANN UCB [0,1] -> (currentValue - minValue) / (maxValue - minValue)
				double normalizedANNRate = (ANNBestUCB - UCBMin) / (MCTSBestUCB - UCBMin);
				double differenceFromMax = 1 - normalizedANNRate;
				double diffSquare = Math.Pow(differenceFromMax, 2);
				mctsReward = (float)(1.3d * Math.Exp(-5.84d * diffSquare) - 0.3d);
			}

			return mctsReward;
		}

	}

}
