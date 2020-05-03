/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorMCTSAgentAction
 *   
 *   Description:    MCTS agent that gives extra reward for selected action
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public class WarriorMCTSAgentAction : WarriorMCTSAgent
	{

		public override void OnActionReceived(float[] vectorAction)
		{
			// Get MCTS action reward before action taken
			monteCarloManager.GetActionReward(player, enemy, vectorAction);

			base.OnActionReceived(vectorAction);
		}

	}

}