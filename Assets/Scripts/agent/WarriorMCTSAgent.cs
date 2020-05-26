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

		private float oldTimeScale;

		public WarriorMCTSAgent() : base("WarriorMCTS", false)
		{
		}

		public override void Start()
		{
			base.Start();
			// Initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy);
			this.oldTimeScale = Time.timeScale;
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
			Time.timeScale = 0;

			// Reward for MCTS result
			float mctsReward = monteCarloManager.GetReward(mctsPlayer, mctsEnemy, vectorAction);

			// Resume game after MCTS finishes
			Time.timeScale = this.oldTimeScale;

			// add %50 of this value
			AddReward(mctsReward / 2f);

			// Request new decision
			RequestDecision();
		}

	}

}