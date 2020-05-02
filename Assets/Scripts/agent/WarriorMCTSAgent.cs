using UnityEngine;

namespace PAPIOnline
{

	public class WarriorMCTSAgent : PlayerAgent
	{

		protected MonteCarloManager monteCarloManager;

		public WarriorMCTSAgent() : base("WarriorMCTS", WarriorProperties.warriorProps, WarriorProperties.warriorSkills, false)
		{
		}

		public override void Start()
		{
			base.Start();
			// initialize monte carlo manager
			this.monteCarloManager = new MonteCarloManager(player, enemy, GiveMCTSReward);
		}

		public void GiveMCTSReward(float mctsReward)
		{
			// give mcts reward
			AddReward(mctsReward);

			// request new decision
			RequestDecision();
		}

	}

}