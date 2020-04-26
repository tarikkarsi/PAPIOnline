using System;
using System.Collections.Generic;

namespace PAPIOnline
{

	/*
	 * Class representing a node in the search tree.
	 * Stores tree search stats for UCB1.
	 */
	public class MonteCarloNode
	{
		public int action;
		public GameState state;
		public MonteCarloNode parent;
		public Dictionary<int, MonteCarloNode> children; // action to node

		public int n_plays;
		public int n_wins;
		public int n_loses;

		/*
		 * Create a new MonteCarloNode in the search tree.
		 * @param {MonteCarloNode} parent - The parent node.
		 * @param {Play} play - Last play played to get to this state.
		 * @param {State} state - The corresponding state.
		 * @param {Play[]} unexpandedPlays - The node's unexpanded child plays.
		 */
		public MonteCarloNode(MonteCarloNode parent, int action, GameState state, bool[] allActions)
		{
			this.action = action;
			this.state = state;

			// Monte Carlo stuff
			this.n_plays = 0;
			this.n_wins = 0;

			// Tree stuff
			this.parent = parent;
			this.children = new Dictionary<int, MonteCarloNode>();

			for (int i = 0; i < allActions.Length; i++)
			{
				if (allActions[i])
				{
					this.children[i] = null;
				}
			}
		}

		/*
		 * Get the MonteCarloNode corresponding to the given play.
		 * @param {number} play - The play leading to the child node.
		 * @return {MonteCarloNode} The child node corresponding to the play given.
		 */
		public MonteCarloNode ChildNode(int action)
		{
			MonteCarloNode child = this.children[action];
			if (child == null)
			{
				throw new Exception("Child not expanded or no such action!");
			}

			return child;
		}

		/*
		 * Expand the specified child play and return the new child node.
		 * Add the node to the array of children nodes.
		 * Remove the play from the array of unexpanded plays.
		 * @param {Play} play - The play to expand.
		 * @param {State} childState - The child state corresponding to the given play.
		 * @param {Play[]} unexpandedPlays - The given child's unexpanded child plays; typically all of them.
		 * @return {MonteCarloNode} The new child node.
		 */
		public MonteCarloNode Expand(int action, GameState childState, bool[] allActions)
		{
			if (!this.children.ContainsKey(action))
			{
				throw new Exception("No such action!");
			}
			MonteCarloNode childNode = new MonteCarloNode(this, action, childState, allActions);
			this.children[action] = childNode;

			return childNode;
		}

		/*
		 * Get all legal plays from this node.
		 * @return {Play[]} All plays.
		 */
		public List<int> AllActions()
		{
			List<int> ret = new List<int>();

			foreach (MonteCarloNode child in this.children.Values)
			{
				if (child != null)
				{
					ret.Add(child.action);
				}
			}

			return ret;
		}

		/*
		 * Get all unexpanded legal plays from this node.
		 * @return {Play[]} All unexpanded plays.
		 */
		public List<int> UnexpandedActions()
		{
			List<int> ret = new List<int>();

			foreach (KeyValuePair<int, MonteCarloNode> child in this.children)
			{
				if (child.Value == null)
				{
					ret.Add(child.Key);
				}
			}

			return ret;
		}

		/*
		 * Whether this node is fully expanded.
		 * @return {boolean} Whether this node is fully expanded.
		 */
		public bool IsFullyExpanded()
		{
			foreach (MonteCarloNode child in this.children.Values)
			{
				if (child == null)
				{
					return false;
				}
			}
			return true;
		}

		/*
		 * Whether this node is terminal in the game tree, NOT INCLUSIVE of termination due to winning.
		 * @return {boolean} Whether this node is a leaf in the tree.
		 */
		public bool IsLeaf()
		{
			return this.children.Count == 0;
		}

		/*
		 * Get the UCB1 value for this node.
		 * @param {number} biasParam - The square of the bias parameter in the UCB1 algorithm, defaults to 2.
		 * @return {number} The UCB1 value of this node.
		 */
		public double GetUCB1(int biasParam)
		{
			return ((double)this.n_wins) / this.n_plays + Math.Sqrt(biasParam * Math.Log(this.parent.n_plays) / this.n_plays);
		}
	}

}