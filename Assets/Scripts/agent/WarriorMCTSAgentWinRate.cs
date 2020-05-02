
namespace PAPIOnline
{

	public class WarriorMCTSAgentWinRate : WarriorMCTSAgent
	{

		public override void OnActionReceived(float[] vectorAction)
		{
			base.OnActionReceived(vectorAction);

			// get MCTS win rate reward after action taken
			monteCarloManager.GetWinRateReward(player, enemy);
		}

	}

}