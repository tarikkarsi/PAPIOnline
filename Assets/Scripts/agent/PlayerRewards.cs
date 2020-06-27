/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          PlayerRewards
 *   
 *   Description:    Helper class for calculating rewards
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public class PlayerRewards
	{

		private static float WIN_REWARD = 1;
		private static float LOSE_REWARD = -1;
		private static float STEP_REWARD;
		private static float DAMAGE_REWARD = 0.002f;
		private static float PROPERTY_REWARD = 0.1f;
		private static float HEALTH_PROPERTY_REWARD = 0.12f;
		private static float MANA_PROPERTY_REWARD = 0.04f;
		private static float DEBUFF_REWARD = 0.1f;
		private static float STUN_DEBUFF_REWARD = 0.1f;

		private string playerName;

		public PlayerRewards(string playerName, int maxStep)
		{
			this.playerName = playerName;
			STEP_REWARD = -1f / maxStep;
		}

		public float GetWinReward()
		{
			return WIN_REWARD;
		}

		public float GetLoseReward()
		{
			return LOSE_REWARD;
		}

		public float GetStepReward()
		{
			return STEP_REWARD;
		}

		public float GetDamageReward(PlayerProperties previousEnemyProperties, PlayerProperties currentEnemyProperties)
		{
			return DAMAGE_REWARD * (previousEnemyProperties.health - currentEnemyProperties.health);
		}

		public float GetDebuffReward(ISet<BuffKind> previousEnemyDebuffs, IList<IBuffSkill> enemyDebuffs)
		{
			float reward = 0f;
			foreach (IBuffSkill debuff in enemyDebuffs)
			{
				// Find the applied debuff
				if (!previousEnemyDebuffs.Contains(debuff.GetBuffKind()))
				{
					reward = debuff.GetBuffKind() == BuffKind.STUN ? STUN_DEBUFF_REWARD : DEBUFF_REWARD;
					break;
				}
			}
			return reward;
		}

		public float GetPropertyReward(PlayerProperties previousPlayerProperties, PlayerProperties currentPlayerProperties)
		{
			float reward = 0f;
			//  Reward for increasing damage, defense, speed
			if (previousPlayerProperties.damage > currentPlayerProperties.damage ||
				previousPlayerProperties.defense > currentPlayerProperties.defense ||
				previousPlayerProperties.speed > currentPlayerProperties.speed)
			{
				reward += PROPERTY_REWARD;
			}
			// Reward for increasing health
			if (previousPlayerProperties.health > currentPlayerProperties.health)
			{
				reward += HEALTH_PROPERTY_REWARD;
			}
			// Reward for increasing mana
			if (previousPlayerProperties.mana > currentPlayerProperties.mana)
			{
				reward += MANA_PROPERTY_REWARD;
			}
			return reward;
		}

		public void Print()
		{
			Debug.Log("*****************************************************");
			Debug.Log(playerName + " rewards:");
			Debug.Log("winReward = " + WIN_REWARD);
			Debug.Log("loseReward = " + LOSE_REWARD);
			Debug.Log("stepReward = " + STEP_REWARD);
			Debug.Log("damageReward = " + DAMAGE_REWARD);
			Debug.Log("debuffReward = " + DEBUFF_REWARD);
			Debug.Log("stunDebuffReward = " + STUN_DEBUFF_REWARD);
			Debug.Log("propertyReward = " + PROPERTY_REWARD);
			Debug.Log("healthPropertyReward = " + HEALTH_PROPERTY_REWARD);
			Debug.Log("manaPropertyReward = " + MANA_PROPERTY_REWARD);
			Debug.Log("*****************************************************");
		}
	}

}