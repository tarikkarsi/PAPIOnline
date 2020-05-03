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

		protected MonteCarloManager monteCarloManager;

		public WarriorMCTSAgent() : base("WarriorMCTS", false)
		{
		}

		public override void Start()
		{
			base.Start();
			// Initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy, GiveMCTSReward);
		}

		public void GiveMCTSReward(float mctsReward)
		{
			// Reward for MCTS result
			AddReward(mctsReward);

			// Request new decision
			RequestDecision();
		}

	}

}