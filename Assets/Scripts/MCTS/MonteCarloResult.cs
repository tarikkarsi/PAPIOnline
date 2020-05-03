/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          MonteCarloResult
 *   
 *   Description:    Class representing a node in the search tree.
 *					 Stores tree search stats for UCB1.
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
namespace PAPIOnline
{

	public class MonteCarloResult
	{
		private int action;
		private int numberOfPlays;
		private int numberOfWins;

		public MonteCarloResult(int action, int numberOfPlays, int numberOfWins)
		{
			this.action = action;
			this.numberOfPlays = numberOfPlays;
			this.numberOfWins = numberOfWins;
		}

		public int GetAction()
		{
			return this.action;
		}

		public int GetNumberOfPlays()
		{
			return this.numberOfPlays;
		}

		public int GetNumberOfWins()
		{
			return this.numberOfWins;
		}
	}

}