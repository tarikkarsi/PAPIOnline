using UnityEngine;

namespace PAPIOnline
{

	public class WarriorMCTSAgentAction : WarriorMCTSAgent
	{

		public override void OnActionReceived(float[] vectorAction)
		{
			// get MCTS action reward before action taken
			monteCarloManager.GetActionReward(player, enemy, vectorAction);

			base.OnActionReceived(vectorAction);
		}

	}

}