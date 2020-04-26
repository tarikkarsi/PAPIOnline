
namespace PAPIOnline
{

	public class WarriorAgentMCTSWinRate : PlayerAgent
	{

		private MonteCarloManager monteCarloManager;
		private volatile bool monteCarloFinished = true;

		public WarriorAgentMCTSWinRate() : base("Warrior", WarriorProperties.warriorProps, WarriorProperties.warriorSkills)
		{
		}

		public override void Start()
		{
			base.Start();
			// initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (monteCarloFinished)
			{
				monteCarloFinished = false;

				// request decision when monte carlo finihed
				RequestDecision();
			}
		}

		public override void OnActionReceived(float[] vectorAction)
		{
			base.OnActionReceived(vectorAction);

			// get MCTS win rate reward after action taken
			monteCarloManager.GetWinRateReward(player, enemy);
		}

		public void OnMonteCarloFinished()
		{
			// give mcts reward
			monteCarloFinished = true;
		}

	}

}