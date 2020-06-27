/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          MonteCarloNode
 *   
 *   Description:    Class representing a node in the search tree.
 *					 Stores tree search stats for UCB1.
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Collections.Generic;

namespace PAPIOnline
{

	public class MonteCarloNode
	{
		public int action;
		public GameState state;
		public MonteCarloNode parent;
		public Dictionary<int, MonteCarloNode> children; // Action to node
		public int numberOfPlays;
		public int numberOfWins;

		public MonteCarloNode(MonteCarloNode parent, int action, GameState state, bool[] allActions)
		{
			this.action = action;
			this.state = state;

			// Monte Carlo stuff
			this.numberOfPlays = 0;
			this.numberOfWins = 0;

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
		 * Get the MonteCarloNode corresponding to the given action.
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
		 * Expand the specified child action and return the new child node.
		 * Add the node to the array of children nodes.
		 * Remove the play from the array of unexpanded actions.
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
		 * Get all legal actions from this node.
		 */
		public List<int> AllActions()
		{
			List<int> allActions = new List<int>();

			foreach (MonteCarloNode child in this.children.Values)
			{
				if (child != null)
				{
					allActions.Add(child.action);
				}
			}

			return allActions;
		}

		/*
		 * Get all unexpanded legal actions from this node.
		 */
		public List<int> UnexpandedActions()
		{
			List<int> unexpandedActions = new List<int>();

			foreach (KeyValuePair<int, MonteCarloNode> child in this.children)
			{
				if (child.Value == null)
				{
					unexpandedActions.Add(child.Key);
				}
			}

			return unexpandedActions;
		}

		/*
		 * Whether this node is fully expanded.
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
		 */
		public bool IsLeaf()
		{
			return this.children.Count == 0;
		}

		/*
		 * Get the UCB1 value for this node.
		 */
		public double GetUCB1(int biasParam)
		{
			return ((double)this.numberOfWins) / this.numberOfPlays + 
			Math.Sqrt(biasParam * Math.Log(this.parent.numberOfPlays) / this.numberOfPlays);
		}
	}

}