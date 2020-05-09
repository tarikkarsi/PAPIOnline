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
		private float numberOfPlays;
		private float numberOfWins;

		public MonteCarloResult(int action, float numberOfPlays, float numberOfWins)
		{
			this.action = action;
			this.numberOfPlays = numberOfPlays;
			this.numberOfWins = numberOfWins;
		}

		public int GetAction()
		{
			return this.action;
		}

		public float GetNumberOfPlays()
		{
			return this.numberOfPlays;
		}

		public float GetNumberOfWins()
		{
			return this.numberOfWins;
		}
	}

}