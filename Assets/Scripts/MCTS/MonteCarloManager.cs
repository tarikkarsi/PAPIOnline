/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          MonteCarloManager
 *   
 *   Description:    Manager that controls MCTS running time and calculates
 *					 rewards according to the result
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PAPIOnline
{

	public class MonteCarloManager
	{
		public float fixedDeltaTime = 0.4f;
		public float searchTimeout = 0.15f;
		public int maxSimulation = 50;
		public int maxDepth = 50;
		public int UCB1ExploreParam = 2;
		private MonteCarlo mcts;
		private Action<float> rewardCallback;

		private string name;

		public MonteCarloManager(IPlayer player, IPlayer enemy, Action<float> rewardCallback)
		{
			this.name = player.GetName();
			this.mcts = new MonteCarlo(new Game(player, enemy, fixedDeltaTime), maxSimulation, maxDepth, UCB1ExploreParam);
			this.rewardCallback = rewardCallback;
		}

		public void CalculateReward(IPlayer player, IPlayer enemy, float[] vectorAction)
		{
			// Reset the MCTS
			this.mcts.Reset(player, enemy);
			Thread t = new Thread(() => {
				this.mcts.RunSearch();
				float reward = CalculateMCTSActionReward(vectorAction);
				this.rewardCallback(reward);
			});
			//t.Priority = ThreadPriority.Highest;
			t.Start();
		}

		public float CalculateMCTSActionReward(float[] vectorAction)
		{
			float mctsReward = 0;
			MonteCarloNode rootNode = mcts.GetRootNode();
			List<int> mctsActions = mcts.GetActionsWithoutMove(rootNode);
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

			// No reward will be given if suitable action not found
			// Move actions eliminated here
			if (ANNBestUCB != Double.NegativeInfinity)
			{
				if (ANNBestUCB == MCTSBestUCB) 
				{
					mctsReward = 1;
				}
				else{
					mctsReward = -1;
				}
				/*
				// Prevent divide by zero assign too little values
				UCBMin = UCBMin == MCTSBestUCB ? 0 : UCBMin;
				MCTSBestUCB = MCTSBestUCB == 0 ? 000000000.1d : MCTSBestUCB;
				ANNBestUCB = ANNBestUCB == 0 ? 000000000.1d : ANNBestUCB;
				// Normalize the ANN UCB [0,1] -> (currentValue - minValue) / (maxValue - minValue)
				double normalizedANNRate = (ANNBestUCB - UCBMin) / (MCTSBestUCB - UCBMin);
				double differenceFromMax = 1 - normalizedANNRate;
				double diffSquare = Math.Pow(differenceFromMax, 2);
				mctsReward = (float)(1.3d * Math.Exp(-5.84d * diffSquare) - 0.01d);
				*/
			}
			else if (mctsActions.Count > 0)
			{
				// Give negative reward for non move actions that mcts does not recommend
				mctsReward = -1f;
			}
			if (name.Equals(BattleArena.RED_AGENT_TAG) && mctsReward != 0)
			{
				Debug.Log(name + " " + mctsReward + " reward given: vectorActions=" + vectorAction[0] + "," + vectorAction[1] + "," + vectorAction[2] +
					" convertedActions=" + String.Join(",", annActions) + " mctsActions=" + String.Join(",", mctsActions));
			}
			return mctsReward;
		}

	}

}
