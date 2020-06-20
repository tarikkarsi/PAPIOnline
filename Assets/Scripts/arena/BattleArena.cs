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

		[HideInInspector]
		public Vector3 rightEdge;
		public Vector3 leftEdge;
		public Vector3 topEdge;
		public Vector3 bottomEdge;

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

		private void SetEdges()
		{
			Vector3 arenaPos = this.transform.position;
			this.rightEdge = new Vector3(arenaPos.x + BattleArena.WIDTH / 2 - 1, 0.5f, 0);
			this.leftEdge = new Vector3(arenaPos.x - BattleArena.WIDTH / 2 + 1, 0.5f, 0);
			this.topEdge = new Vector3(0, 0.5f, arenaPos.z + BattleArena.HEIGHT / 2 - 1);
			this.bottomEdge = new Vector3(0, 0.5f, arenaPos.z - BattleArena.HEIGHT / 2 + 1);
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

		public void PauseGame()
		{
			this.redAgent.PauseAgent();
			this.blueAgent.PauseAgent();
		}

		public void ResumeGame()
		{
			this.redAgent.ResumeAgent();
			this.blueAgent.ResumeAgent();
		}

	}

}