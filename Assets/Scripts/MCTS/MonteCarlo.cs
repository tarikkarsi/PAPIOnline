/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          MonteCarlo
 *   
 *   Description:    Monte Carlo Tree Search impemenation. Handles the four MCTS
 *					 steps: selection, expansion, simulation, backpropagation.
 *					 Handles best-move selection.
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PAPIOnline
{

    public class MonteCarlo
    {
        private Game game;
        private int UCB1ExploreParam;
        private Dictionary<int, MonteCarloNode> nodes;
        private volatile bool mctsRun;
        private int maxDepth;
        private Action mctsFinished;
        private static readonly CountdownEvent countdown = new CountdownEvent(2);

        public MonteCarlo(Game game, int maxDepth, Action mctsFinished, int UCB1ExploreParam = 2)
        {
            this.game = game;
            this.maxDepth = maxDepth;
            this.mctsFinished = mctsFinished;
            this.UCB1ExploreParam = UCB1ExploreParam;
            this.nodes = new Dictionary<int, MonteCarloNode>();
        }

		public void Reset(IPlayer player, IPlayer enemy)
		{
            this.nodes.Clear();
            this.game.Reset(player, enemy);
        }

        public void MakeNode(GameState state)
        {
            if (!this.nodes.ContainsKey(state.GetHashCode()))
            {
                bool[] unexpandedActions = this.game.LegalActions(state);
                MonteCarloNode node = new MonteCarloNode(null, -1, state, unexpandedActions);
                this.nodes[state.GetHashCode()] = node;
            }
        }

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
			// Run until time runs out
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
		 * Phase 1: Selection
		 * Select until EITHER not fully expanded OR leaf node
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
		 */
        public MonteCarloNode Expand(MonteCarloNode node)
        {
            // Select random action
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
		 */
        public PlayerKind Simulate(MonteCarloNode node, int maxDepth)
        {
            GameState state = node.state;
            IPlayer player = state.GetPlayer().ClonePlayer();
            IPlayer enemy = state.GetEnemy().ClonePlayer();
            PlayerKind turn = state.GetTurn();
            PlayerKind winner;
            int depth = 0;

            // Continue until someone wins, specific depth is reached or time is up
            while ((winner = this.game.Winner(player, enemy)) == PlayerKind.NONE && depth < maxDepth && mctsRun)
            {
                int action = this.game.RandomLegalAction(player, enemy, turn);
                turn = this.game.MakeAction(player, enemy, action, turn);
                this.game.UpdatePlayers(player, enemy);
                depth++;
            }

            // Calculate winner manually if no one wins
            if (winner == PlayerKind.NONE)
            {
                winner = CalculateWinner(state.GetPlayer(), player, state.GetEnemy(), enemy);
            }

            return winner;
        }

        /*
		 * Phase 4: Backpropagation
		 * From given node, propagate plays and winner to ancestors' statistics
		 */
        public void Backpropagate(MonteCarloNode node, PlayerKind winner)
        {
            while (node != null)
            {
                node.numberOfPlays++;
                if (winner == PlayerKind.PLAYER)
                {
                    node.numberOfWins++;
                }
                else if (winner == PlayerKind.ENEMY)
                {
                    node.numberOfLoses++;
                }
                // Parent's choice
                node = node.parent;
            }
        }

        // Utility methods

        public MonteCarloNode GetRootNode()
        {
             return this.nodes[game.GetInitialState().GetHashCode()];
        }

        public ISet<int> ConvertMCTSActions(float[] vectorAction)
        {
            return this.game.ConvertMCTSActions(vectorAction);
        }

        private PlayerKind CalculateWinner(IPlayer playerFirstState, IPlayer playerLastState,
			IPlayer enemyFirstState, IPlayer enemyLastState)
        {
            // Calculate the remaining time with dividing remaining health to health difference
            float playerHealthDiff = playerFirstState.GetHealth() - playerLastState.GetHealth();
            float enemyHealthDiff = enemyFirstState.GetHealth() - enemyLastState.GetHealth();

            float playerRemainingTime = playerLastState.GetHealth() / playerHealthDiff;
            float enemyRemainingTime = enemyLastState.GetHealth() / enemyHealthDiff;
            return playerRemainingTime >= enemyRemainingTime ? PlayerKind.PLAYER : PlayerKind.ENEMY;
        }

        private PlayerKind CalculateWinner2(IPlayer playerFirstState, IPlayer playerLastState,
            IPlayer enemyFirstState, IPlayer enemyLastState)
        {
            float playerTotalHealth = playerLastState.GetHealth() + playerLastState.GetHealthPotionCount() * PlayerProperties.HEALTH_POTION_FILL;
            float playerTotalMana = playerLastState.GetMana() + playerLastState.GetManaPotionCount() * PlayerProperties.MANA_POTION_FILL;
            float enemyTotalHealth = enemyLastState.GetHealth() + enemyLastState.GetHealthPotionCount() * PlayerProperties.HEALTH_POTION_FILL;
            float enemyTotalMana = enemyLastState.GetMana() + enemyLastState.GetManaPotionCount() * PlayerProperties.MANA_POTION_FILL;
            return playerTotalHealth + playerTotalMana >= enemyTotalHealth + enemyTotalMana ? PlayerKind.PLAYER : PlayerKind.ENEMY;
        }

    }

}