# PAPIOnline


Until now, mankind has found solutions to many complex problems by using their mind. Partially observable environments are among these problems. In time, they learn what they have to do in an environment which they cannot fully control. They act by making predictions with their experiences. Developing artificial intelligence techniques take this behavior as an example. Reinforcement learning method tries to reach the best solution with trial and error. It associates the actions  with the observations from and tries to maximize cumulative reward. Since the number of observations that can be obtained in a partially observable environment is limited, it is very difficult to establish this relationship. Therefore, the reward mechanism becomes extremely important. The agent, which has to fulfill many tasks in order to reach the target, must be guided by reward shaping. Reward functions are getting more and more complicated because the importance of actions and tasks change dynamically. It is very difficult to maintain and reuse such calculations.


In this thesis, we present a method that can replace complex reward calculations for partially observable environments. We have created a system that can feed reinforcement learning with rewards from Monte Carlo Tree Search, which is a heuristic search algorithm. When the agent does the best action in a situation, it should get the highest reward. So, how do we interpret that action as the best action? Since the same action may produce different results in different situations, a reward should be given according to the result. It is a daunting task to evaluate and prioritize the results. The MCTS algorithm finds the best action of a given state with its forward simulations. While doing this, it simulates the game many times and evaluates these results. So the result-oriented approach we just mentioned is also valid for MCTS. We created a system for evaluating actions which are taken by RL with MCTS. After each action, we ran MCTS to find best action for current state. Then we gave a positive reward to RL if selected action is same as MCTS's best action, orherwise, we gave a negative reward.


In order to test the method we developed, we chose the Payer versus Player feature of MMORPG game, which is a partially observable environment. We trained same agent with self-play deep reinforcement learning with different reward strategies on a sample MMORPG implementation we wrote. The first agent received manual rewards and the second received rewards according to the MCTS result. Then, we had them battle each other. As the results show that, MCTS strategy outperformed the manual one. 

## Dependency Versions

Unity v2019.3.13f1
ML-Agents v1.0.2
Tensorflow v2.0.2
