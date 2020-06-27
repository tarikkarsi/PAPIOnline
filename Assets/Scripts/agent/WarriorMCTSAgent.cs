/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorMCTSAgent
 *   
 *   Description:    Warrior agent base for monte carlo tree search
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;

namespace PAPIOnline
{

	public class WarriorMCTSAgent : WarriorAgent
	{

		private MonteCarloManager monteCarloManager;

		// MCTS Agent has own request decision mechanism and does not give manual rewards
		public WarriorMCTSAgent() : base("WarriorMCTS", false, false)
		{
		}

		public override void Start()
		{
			base.Start();
			// Initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy, OnRewardReceived);
		}

		public void OnRewardReceived(float mctsReward)
		{
			// Add %50 of this value
			AddReward(mctsReward / 2f);

			// Request new decision
			RequestDecision();

			// Resume the game after MCTS finishes
			this.battleArena.ResumeGame();
		}

		public override void OnActionReceived(float[] vectorAction)
		{
			// MCTS action reward should be calculated with the state before action done
			// So save the current state of player and enemy
			IPlayer mctsPlayer = player.ClonePlayer();
			IPlayer mctsEnemy = enemy.ClonePlayer();

			// Continue ordinary action process 
			base.OnActionReceived(vectorAction);

			// Pause the game when MCTS runs
			this.battleArena.PauseGame();

			// Reward for MCTS result
			this.monteCarloManager.CalculateReward(mctsPlayer, mctsEnemy, vectorAction);
		}

	}

}