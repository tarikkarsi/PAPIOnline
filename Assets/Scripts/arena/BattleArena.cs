using UnityEngine;
using MLAgents;
using TMPro;

public class BattleArena : MonoBehaviour
{
	public string agentBlueTag = "agentBlue";
	public string agentRedTag = "agentRed";
	public string rewardBlueTag = "rewardBlue";
	public string rewardRedTag = "rewardRed";

	private PlayerAgent playerAgentBlue;
	private PlayerAgent playerAgentRed;

	private TextMeshPro cumulativeRewardTextBlue;
	private TextMeshPro cumulativeRewardTextRed;

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
				this.cumulativeRewardTextBlue = tr.GetComponent<TextMeshPro>();
			}
			else if (tr.tag.Equals(rewardRedTag))
			{
				this.cumulativeRewardTextRed = tr.GetComponent<TextMeshPro>();
			}
		}
	}

	public PlayerAgent getRival(string playerTag)
	{
		return playerTag.Equals(agentBlueTag) ? playerAgentRed : playerAgentBlue;
	}

	public void ResetArea()
	{
		ResetPlayers();
	}

	public void ResetTexts()
	{
		cumulativeRewardTextBlue.text = "";
		cumulativeRewardTextRed.text = "";
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
		return (int) UnityEngine.Random.Range(0f, (float)corners.Length);
	}

	private void Update()
	{
		cumulativeRewardTextBlue.text = playerAgentBlue.GetCumulativeReward().ToString();
		cumulativeRewardTextRed.text = playerAgentRed.GetCumulativeReward().ToString();
	}

}
