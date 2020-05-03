/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorMCTSAgentWinRate
 *   
 *   Description:    MCTS agent that gives extra reward for win rate 
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public class WarriorMCTSAgentWinRate : WarriorMCTSAgent
	{

		public override void OnActionReceived(float[] vectorAction)
		{
			base.OnActionReceived(vectorAction);

			// Get MCTS win rate reward after action taken
			monteCarloManager.GetWinRateReward(player, enemy);
		}

	}

}