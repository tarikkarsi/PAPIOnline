using UnityEngine;
using MLAgents;
using TMPro;

public class BattleArena : Area
{
	public PlayerAgent playerAgent1;
	public PlayerAgent playerAgent2;

	public PlayerProperties player1Properties;
	public PlayerProperties player2Properties;

	public TextMeshPro cumulativeRewardText1;
	public TextMeshPro cumulativeRewardText2;

	private readonly Vector3[] corners = { 
		new Vector3(40f, 5f, 40f), 
		new Vector3(40f, 5f, -40f),
		new Vector3(-40f, 5f, 40f),
		new Vector3(-40f, 5f, -40f)
	};

	public override void ResetArea()
	{
		ResetPlayers();
	}

	private void ResetPlayers()
	{
		int corner1Index = ChooseRandomCornerIndex();
		Vector3 player1Pos = corners[corner1Index];
		playerAgent1.SetPosition(player1Pos);

		int corner2Index = corners.Length - corner1Index - 1;
		Vector3 player2Pos = corners[corner2Index];
		playerAgent2.SetPosition(player2Pos);
	}

	private int ChooseRandomCornerIndex()
	{
		return (int) UnityEngine.Random.Range(0f, (float)corners.Length);
	}

	private void Update()
	{
		cumulativeRewardText1.text = playerAgent1.GetCumulativeReward().ToString();
		cumulativeRewardText2.text = playerAgent2.GetCumulativeReward().ToString();
	}

}
