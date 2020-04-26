using UnityEngine;
using MLAgents;
using TMPro;

namespace PAPIOnline
{

	public class BattleArena : MonoBehaviour
	{
		public string agentBlueTag = "agentBlue";
		public string agentRedTag = "agentRed";
		public string rewardBlueTag = "rewardBlue";
		public string rewardRedTag = "rewardRed";

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
				if (tr.tag.Equals(agentBlueTag))
				{
					this.playerAgentBlue = tr.GetComponent<PlayerAgent>();
				}
				else if (tr.tag.Equals(agentRedTag))
				{
					this.playerAgentRed = tr.GetComponent<PlayerAgent>();
				}
				else if (tr.tag.Equals(rewardBlueTag))
				{
					this.rewardTextBlue = tr.GetComponent<TextMeshPro>();
				}
				else if (tr.tag.Equals(rewardRedTag))
				{
					this.rewardTextRed = tr.GetComponent<TextMeshPro>();
				}
			}
		}

		public PlayerAgent GetRival(string playerTag)
		{
			return playerTag.Equals(agentBlueTag) ? playerAgentRed : playerAgentBlue;
		}

		public TextMeshPro GetRewardText(string playerTag)
		{
			return playerTag.Equals(agentBlueTag) ? rewardTextBlue : rewardTextRed;
		}

		public void ResetArea()
		{
			ResetPlayers();
		}

		private void ResetPlayers()
		{
			int corner1Index = ChooseRandomCornerIndex();
			Vector3 player1Pos = corners[corner1Index];
			playerAgentBlue.SetPosition(player1Pos);

			int corner2Index = corners.Length - corner1Index - 1;
			Vector3 player2Pos = corners[corner2Index];
			playerAgentRed.SetPosition(player2Pos);
		}

		private int ChooseRandomCornerIndex()
		{
			return UnityEngine.Random.Range(0, corners.Length);
		}

	}

}