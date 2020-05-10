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
using Unity.MLAgents;
using TMPro;

namespace PAPIOnline
{

	public class BattleArena : MonoBehaviour
	{
		private const string AGENT_BLUE_TAG = "agentBlue";
		private const string AGENT_RED_TAG = "agentRed";
		private const string REWARD_BLUE_TAG = "rewardBlue";
		private const string REWARD_RED_TAG = "rewardRed";

		private PlayerAgent playerAgentBlue;
		private PlayerAgent playerAgentRed;

		private TextMeshPro rewardTextBlue;
		private TextMeshPro rewardTextRed;

		private readonly Vector3[] corners = {
		new Vector3(30f, 0f, 30f),
		new Vector3(30f, 0f, -30f),
		new Vector3(-30f, 0f, 30f),
		new Vector3(-30f, 0f, -30f)
	};

		public void Awake()
		{
			SetComponents();
			Academy.Instance.OnEnvironmentReset += ResetArea;
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

		public void ResetArea()
		{
			ResetPlayers();
		}

		private void ResetPlayers()
		{
			int corner1Index = ChooseRandomCornerIndex();
			// place relative to area
			Vector3 player1Pos = corners[corner1Index] + this.transform.position;
			playerAgentBlue.SetPosition(player1Pos);

			int corner2Index = corners.Length - corner1Index - 1;
			Vector3 player2Pos = corners[corner2Index] + this.transform.position;
			playerAgentRed.SetPosition(player2Pos);
		}

		private int ChooseRandomCornerIndex()
		{
			return Random.Range(0, corners.Length);
		}

	}

}