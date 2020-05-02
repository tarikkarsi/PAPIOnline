using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PAPIOnline
{

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
        private volatile bool mctsRun;
        private int maxDepth;
        private Action mctsFinished;
        private static readonly CountdownEvent countdown = new CountdownEvent(2);

        /*
		 * Create a Monte Carlo search tree.
		 * @param {Game} game - The game to query regarding legal moves and state advancement.
		 * @param {number} UCB1ExploreParam - The square of the bias parameter in the UCB1 algorithm; defaults to 2.
		 */
        public MonteCarlo(Game game, int maxDepth, Action mctsFinished, int UCB1ExploreParam = 2)
        {
            this.game = game;
            this.maxDepth = maxDepth;
            this.mctsFinished = mctsFinished;
            this.UCB1ExploreParam = UCB1ExploreParam;
            this.nodes = new Dictionary<int, MonteCarloNode>();
        }

        /*
		 * If state does not exist, create dangling node.
		 * @param {State} state - The state to make a dangling node for; its parent is set to null.
		 */
        public void MakeNode(GameState state)
        {
            if (!this.nodes.ContainsKey(state.GetHashCode()))
            {
                bool[] unexpandedActions = this.game.LegalActions(state);
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
        public void RunSearch()
        {
            mctsRun = true;
            GameState state = game.GetInitialState();

            if (state.GetPlayer().GetName().Equals("agentBlue"))
            {
                countdown.Reset();
            }

            this.MakeNode(state);
            int totalSims = 0;
            while (mctsRun)
            {
                MonteCarloNode node = this.Select(state);
                PlayerKind winner = this.game.Winner(node.state);

                if (node.IsLeaf() == false && winner == PlayerKind.NONE)
                {
                    node = this.Expand(node);
                    winner = this.Simulate(node, maxDepth);
                }
                this.Backpropagate(node, winner);

                totalSims++;
            }

            countdown.Signal();
            countdown.Wait();

            mctsFinished();
        }


        public async void EndSearch()
        {
            await Task.Delay(15);
            mctsRun = false;
        }

        /*
		 * From the available statistics, calculate the best move from the given state.
		 * @param {State} state - The state to get the best play from.
		 * @param {string} policy - The selection policy for the "best" play.
		 * @return {Play} The best play, according to the given policy.
		 */
        public int BestAction(GameState state)
        {
            MonteCarloNode node = this.nodes[state.GetHashCode()];
            List<int> allActions = node.AllActions();
            int bestAction = -1;

            double max = -1;
            foreach (int action in allActions)
            {
                MonteCarloNode childNode = node.ChildNode(action);
                if (childNode != null)
                {
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
                List<int> actions = node.AllActions();
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
            List<int> actions = node.UnexpandedActions();
            int action = this.game.RandomAction(actions);
            GameState childState = this.game.NextState(node.state, action);
            bool[] childActions = this.game.LegalActions(childState);
            MonteCarloNode childNode = node.Expand(action, childState, childActions);
            this.nodes[childState.GetHashCode()] = childNode;
            return childNode;
        }

        /*
		 * Phase 3: Simulation
		 * From given node, play the game until a terminal state, then return winner
		 * @param {MonteCarloNode} node - The node to simulate from.
		 * @return {number} The winner of the terminal game state.
		 */
        public PlayerKind Simulate(MonteCarloNode node, int maxDepth)
        {
            GameState state = node.state;
            IPlayer player = state.GetPlayer().ClonePlayer();
            IPlayer enemy = state.GetEnemy().ClonePlayer();
            PlayerKind turn = state.GetTurn();
            PlayerKind winner;
            int depth = 0;

            // continue until someone wins, specific depth is reached or time is up
            while ((winner = this.game.Winner(player, enemy)) == PlayerKind.NONE && depth < maxDepth && mctsRun)
            {
                int action = this.game.RandomLegalAction(player, enemy, turn);
                turn = this.game.MakeAction(player, enemy, action, turn);
                this.game.UpdatePlayers(player, enemy);
                depth++;
            }

            // calculate winner manually if no one wins
            if (winner == PlayerKind.NONE)
            {
                winner = CalculateWinner(state.GetPlayer(), player, state.GetEnemy(), enemy);
            }

            return winner;
        }



        private PlayerKind CalculateWinner(IPlayer playerFirstState, IPlayer playerLastState,
            IPlayer enemyFirstState, IPlayer enemyLastState)
        {
            // calculate the remaining time with dividing remaining health to health difference
            float playerHealthDiff = playerFirstState.GetHealth() - playerLastState.GetHealth();
            float enemyHealthDiff = enemyFirstState.GetHealth() - enemyLastState.GetHealth();

            float playerRemainingTime = playerLastState.GetHealth() / playerHealthDiff;
            float enemyRemainingTime = enemyLastState.GetHealth() / enemyHealthDiff;
            return playerRemainingTime >= enemyRemainingTime ? PlayerKind.PLAYER : PlayerKind.ENEMY;
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
        public MonteCarloResult GetResult(GameState state)
        {
            MonteCarloNode node = this.nodes[state.GetHashCode()];
            return new MonteCarloResult(node.action, node.n_plays, node.n_wins);
        }
    }

}