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

namespace PAPIOnline
{

	public class BattleArena : MonoBehaviour
	{
		[HideInInspector]
		public static int WIDTH = 50;
		public static int HEIGHT = 50;
		public static int MAX_DISTANCE = 70; // near hypotenus
		public const string BLUE_AGENT_TAG = "blueAgent";
		public const string BLUE_INFO_TAG = "blueInfo";
		public const string RED_AGENT_TAG = "redAgent";
		public const string RED_INFO_TAG = "redInfo";

		private PlayerAgent blueAgent;
		private PlayerAgent redAgent;
		private BattleInfo blueBattleInfo;
		private BattleInfo redBattleInfo;

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
					case BLUE_AGENT_TAG:
						this.blueAgent = tr.GetComponent<PlayerAgent>();
						break;
					case RED_AGENT_TAG:
						this.redAgent = tr.GetComponent<PlayerAgent>();
						break;
					case BLUE_INFO_TAG:
						this.blueBattleInfo = tr.GetComponent<BattleInfo>();
						break;
					case RED_INFO_TAG:
						this.redBattleInfo = tr.GetComponent<BattleInfo>();
						break;
				}
			}
		}

		public PlayerAgent GetRival(string playerTag)
		{
			return playerTag.Equals(BLUE_AGENT_TAG) ? redAgent : blueAgent;
		}

		public BattleInfo GetBattleInfo(string playerTag)
		{
			return playerTag.Equals(BLUE_AGENT_TAG) ? blueBattleInfo : redBattleInfo;
		}

		public Vector3 GetPosition()
		{
			return this.transform.position;
		}

	}

}