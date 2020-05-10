/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          BattleArena
 *   
 *   Description:    Arena that contains players and reward texts
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using TMPro;

namespace PAPIOnline
{

	public class BattleArena : MonoBehaviour
	{
		[HideInInspector]
		public const string AGENT_BLUE_TAG = "agentBlue";
		public const string AGENT_RED_TAG = "agentRed";
		private const string REWARD_BLUE_TAG = "rewardBlue";
		private const string REWARD_RED_TAG = "rewardRed";

		private PlayerAgent playerAgentBlue;
		private PlayerAgent playerAgentRed;

		private TextMeshPro rewardTextBlue;
		private TextMeshPro rewardTextRed;

		public void Awake()
		{
			SetComponents();
		}

		private void SetComponents()
		{
			Transform t = this.transform;
			foreach (Transform tr in t)
			{
				switch (tr.tag)
				{
					case AGENT_BLUE_TAG:
						this.playerAgentBlue = tr.GetComponent<PlayerAgent>();
						break;
					case AGENT_RED_TAG:
						this.playerAgentRed = tr.GetComponent<PlayerAgent>();
						break;
					case REWARD_BLUE_TAG:
						this.rewardTextBlue = tr.GetComponent<TextMeshPro>();
						break;
					case REWARD_RED_TAG:
						this.rewardTextRed = tr.GetComponent<TextMeshPro>();
						break;
					default:
						break;
				}
			}
		}

		public PlayerAgent GetRival(string playerTag)
		{
			return playerTag.Equals(AGENT_BLUE_TAG) ? playerAgentRed : playerAgentBlue;
		}

		public TextMeshPro GetRewardText(string playerTag)
		{
			return playerTag.Equals(AGENT_BLUE_TAG) ? rewardTextBlue : rewardTextRed;
		}

		public Vector3 GetPosition()
		{
			return this.transform.position;
		}

	}

}