
namespace PAPIOnline
{

	public class GameState
	{
		public static int HASH_CODE = 0;

		private IPlayer player;
		private IPlayer enemy;
		private PlayerKind playerKind;
		private readonly int hashCode;

		public GameState(IPlayer player, IPlayer enemy, PlayerKind playerKind)
		{
			this.player = player;
			this.enemy = enemy;
			this.playerKind = playerKind;
			// increment hash code each state creation
			this.hashCode = HASH_CODE++;
		}

		public bool IsPlayer()
		{
			return this.playerKind == PlayerKind.PLAYER;
		}

		public PlayerKind GetTurn()
		{
			return this.playerKind;
		}

		public IPlayer GetPlayer()
		{
			return this.player;
		}

		public IPlayer GetEnemy()
		{
			return this.enemy;
		}

		public override int GetHashCode()
		{
			return this.hashCode;
		}
	}

}