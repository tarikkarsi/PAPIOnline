using UnityEngine;

namespace PAPIOnline
{

	public class WarriorAgentMCTSAction : PlayerAgent
	{

		private MonteCarloManager monteCarloManager;

		private volatile bool monteCarloFinished = true;

		public WarriorAgentMCTSAction() : base("Warrior", WarriorProperties.warriorProps, WarriorProperties.warriorSkills)
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
			// get MCTS action reward before action taken
			monteCarloManager.GetActionReward(player, enemy, vectorAction, OnMonteCarloFinished);

			base.OnActionReceived(vectorAction);
		}

		public void OnMonteCarloFinished()
		{
			// give mcts reward
			monteCarloFinished = true;
		}

	}

}