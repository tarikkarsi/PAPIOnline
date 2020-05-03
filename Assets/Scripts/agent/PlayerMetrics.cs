/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          PlayerMetrics
 *   
 *   Description:    Container class for saving required metrics for player
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System.Collections.Generic;

namespace PAPIOnline
{

	public class PlayerMetrics
	{
		public PlayerProperties properties;
		public ISet<BuffKind> debuffs = new HashSet<BuffKind>();

		public void Set(IPlayer player)
		{
			properties = player.GetProperties().Clone();
			debuffs.Clear();
			foreach (IBuffSkill debuff in player.GetAppliedDebuffs())
			{
				debuffs.Add(debuff.GetBuffKind());
			}
			
		}

	}

}