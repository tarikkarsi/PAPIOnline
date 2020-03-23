using System;
using System.Collections.Generic;

/*
 * Class representing the Monte Carlo search tree.
 * Handles the four MCTS steps: selection, expansion, simulation, backpropagation.
 * Handles best-move selection.
 */
public class MonteCarlo
{
	private Game game;
	private int UCB1ExploreParam;
	private Dictionary<int, MonteCarloNode> nodes;
	private Random random;

	/*
	 * Create a Monte Carlo search tree.
	 * @param {Game} game - The game to query regarding legal moves and state advancement.
	 * @param {number} UCB1ExploreParam - The square of the bias parameter in the UCB1 algorithm; defaults to 2.
	 */
	public MonteCarlo(Game game, int UCB1ExploreParam = 2)
	{
		this.game = game;
		this.UCB1ExploreParam = UCB1ExploreParam;
		this.nodes = new Dictionary<int, MonteCarloNode>();
		this.random = new Random();
	}

	/*
	 * If state does not exist, create dangling node.
	 * @param {State} state - The state to make a dangling node for; its parent is set to null.
	 */
	public void MakeNode(GameState state)
	{
		if (!this.nodes.ContainsKey(state.GetHashCode()))
		{
			int[] unexpandedActions = this.game.LegalActions(state);
			MonteCarloNode node = new MonteCarloNode(null, -1, state, unexpandedActions);
			this.nodes[state.GetHashCode()] = node;
		}
	}

	/*
	 * From given state, run as many simulations as possible until the time limit, building statistics.
	 * @param {State} state - The state to run the search from.
	 * @param {number} timeout - The time to run the simulations for, in seconds.
	 * @return {Object} Search statistics.
	 */
	public SearchResult RunSearch(GameState state, float timeout)
	{
		this.MakeNode(state);

		int totalSims = 0;

		float end = DateTime.Now.Millisecond + (timeout * 1000);

		while (DateTime.Now.Millisecond < end)
		{
			MonteCarloNode node = this.Select(state);
			PlayerKind winner = this.game.Winner(node.state);

			if (node.IsLeaf() == false && winner == PlayerKind.NONE)
			{
				node = this.Expand(node);
				winner = this.Simulate(node);
			}
			this.Backpropagate(node, winner);

			totalSims++;
		}

		return new SearchResult(timeout, totalSims);
	}

	/*
	 * From the available statistics, calculate the best move from the given state.
	 * @param {State} state - The state to get the best play from.
	 * @param {string} policy - The selection policy for the "best" play.
	 * @return {Play} The best play, according to the given policy.
	 */
	public int BestAction(GameState state, BestPlaySelectionPolicy policy = BestPlaySelectionPolicy.ROBUST)
	{
		this.MakeNode(state);
		// If not all children are expanded, not enough information
		if (this.nodes[state.GetHashCode()].IsFullyExpanded() == false)
			throw new Exception("Not enough information!");

		MonteCarloNode node = this.nodes[state.GetHashCode()];
		int[] allActions = node.AllActions();
		int bestAction = -1;

		if (policy == BestPlaySelectionPolicy.ROBUST)
		{
			int max = -1;
			foreach (int action in allActions)
			{
				MonteCarloNode childNode = node.ChildNode(action);
				if (childNode.n_plays > max)
				{
					bestAction = action;
					max = childNode.n_plays;
				}
			}
		}
		else if (policy == BestPlaySelectionPolicy.MAX)
		{
			double max = -1;
			foreach (int action in allActions)
			{
				MonteCarloNode childNode = node.ChildNode(action);
				double ratio = ((double)childNode.n_plays) / childNode.n_plays;
				if (ratio > max)
				{
					bestAction = action;
					max = ratio;
				}
			}
		}

		return bestAction;
	}

	/*
	 * Phase 1: Selection
	 * Select until EITHER not fully expanded OR leaf node
	 * @param {State} state - The root state to start selection from.
	 * @return {MonteCarloNode} The selected node.
	 */
	public MonteCarloNode Select(GameState state)
	{
		MonteCarloNode node = this.nodes[state.GetHashCode()];

		while (node.IsFullyExpanded() && !node.IsLeaf())
		{
			int[] actions = node.AllActions();
			int bestAction = -1;
			double bestUCB1 = Double.NegativeInfinity;

			foreach (int action in actions)
			{
				double childUCB1 = node.ChildNode(action).GetUCB1(this.UCB1ExploreParam);
				if (childUCB1 > bestUCB1)
				{
					bestAction = action;
					bestUCB1 = childUCB1;

				}
			}
			node = node.ChildNode(bestAction);
		}
		return node;
	}

	/*
	 * Phase 2: Expansion
	 * Of the given node, expand a random unexpanded child node
	 * @param {MonteCarloNode} node - The node to expand from. Assume not leaf.
	 * @return {MonteCarloNode} The new expanded child node.
	 */
	public MonteCarloNode Expand(MonteCarloNode node)
	{
		// select random action
		int[] actions = node.UnexpandedActions();
		int action = actions[this.random.Next(0, actions.Length)];

		GameState childState = this.game.NextState(node.state, action);
		int[] childUnexpandedPlays = this.game.LegalActions(childState);
		MonteCarloNode childNode = node.Expand(action, childState, childUnexpandedPlays);
		this.nodes[childState.GetHashCode()] = childNode;

		return childNode;
	}

	/*
	 * Phase 3: Simulation
	 * From given node, play the game until a terminal state, then return winner
	 * @param {MonteCarloNode} node - The node to simulate from.
	 * @return {number} The winner of the terminal game state.
	 */
	public PlayerKind Simulate(MonteCarloNode node)
	{
		GameState state = node.state;
		PlayerKind winner = this.game.Winner(state);

		while (winner == PlayerKind.NONE)
		{
			int[] actions = this.game.LegalActions(state);
			int action = actions[this.random.Next(0, actions.Length)];
			state = this.game.NextState(state, action);
			winner = this.game.Winner(state);
		}

		return winner;
	}

	/*
	 * Phase 4: Backpropagation
	 * From given node, propagate plays and winner to ancestors' statistics
	 * @param {MonteCarloNode} node - The node to backpropagate from. Typically leaf.
	 * @param {number} winner - The winner to propagate.
	 */
	public void Backpropagate(MonteCarloNode node, PlayerKind winner)
	{
		while (node != null)
		{
			node.n_plays++;
			// Parent's choice
			if (winner == PlayerKind.PLAYER)
			{
				node.n_wins++;
			}
			else if (winner == PlayerKind.ENEMY)
			{
				node.n_loses++;
			}
			node = node.parent;
		}
	}

	// Utility & debugging methods

	/*
	 * Return MCTS statistics for this node and children nodes
	 * @param {State} state - The state to get statistics for.
	 * @return {Object} The MCTS statistics.
	 */
	public Stats GetStats(GameState state)
	{
		MonteCarloNode node = this.nodes[state.GetHashCode()];
		Stats stats = new Stats(node.action, node.n_plays, node.n_wins);

		foreach (MonteCarloNode child in node.children.Values)
		{
			if (child == null)
			{
				stats.children.Add(new Stats(-1, 0, 0));
			}
			else
			{
				stats.children.Add(new Stats(child.action, child.n_plays, child.n_wins));
			}
		}
		return stats;
	}
}