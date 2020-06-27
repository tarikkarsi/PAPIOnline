/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          GameState
 *   
 *   Description:    Container for holding player and enemy state
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public class GameState
	{
		private int id = 0;
		private IPlayer player;
		private IPlayer enemy;
		private PlayerKind playerKind;
		private readonly int hashCode;

		public GameState(int id, IPlayer player, IPlayer enemy, PlayerKind playerKind)
		{
			this.player = player;
			this.enemy = enemy;
			this.playerKind = playerKind;
			this.id = id;
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

		public int GetId()
		{
			return this.id;
		}
	}

}