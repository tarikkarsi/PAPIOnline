
namespace PAPIOnline
{

	public class MonteCarloResult
	{
		private int action;
		private int playCount;
		private int winCount;

		public MonteCarloResult(int action, int playCount, int winCount)
		{
			this.action = action;
			this.playCount = playCount;
			this.winCount = winCount;
		}

		public int GetAction()
		{
			return this.action;
		}

		public int GetPlayCount()
		{
			return this.playCount;
		}

		public int GetWinCount()
		{
			return this.winCount;
		}
	}

}