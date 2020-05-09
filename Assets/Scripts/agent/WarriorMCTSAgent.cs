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

namespace PAPIOnline
{

	public class WarriorMCTSAgent : WarriorAgent
	{

		public MonterCarloRewardType rewardType;
		private MonteCarloManager monteCarloManager;

		public WarriorMCTSAgent() : base("WarriorMCTS", false)
		{
		}

		public override void Start()
		{
			base.Start();
			// Initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy, GiveMCTSReward, rewardType);
		}

		public void GiveMCTSReward(float mctsReward)
		{
			// Reward for MCTS result
			// add %50 of this value
			AddReward(mctsReward / 2);

			UnityEngine.Debug.LogError("MCTS Reward = " + (mctsReward / 2));

			// Request new decision
			RequestDecision();
		}

		public override void OnActionReceived(float[] vectorAction)
		{
			if (rewardType == MonterCarloRewardType.ACTION)
			{
				// Get MCTS action reward before action taken
				monteCarloManager.GetReward(player, enemy, vectorAction);
				base.OnActionReceived(vectorAction);
			}
			else
			{
				// Get MCTS win rate reward after action taken
				base.OnActionReceived(vectorAction);
				monteCarloManager.GetReward(player, enemy, vectorAction);
			}
			
		}

	}

}