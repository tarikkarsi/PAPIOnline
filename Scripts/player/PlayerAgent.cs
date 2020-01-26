using System;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PlayerAgent : Agent
{
	private static int MOVE_BRANCH_INDEX = 0;
	private static int SKILL_BRANCH_INDEX = 1;

	public PlayerAgent enemyAgent;

	private IPlayer player;
	private IPlayer enemy;

	private BattleArena battleArena;
	private Rigidbody agentRB;
	// private RayPerception3D rayPerception

	private PlayerMetrics previousOwnMetrics = new PlayerMetrics();
	private PlayerMetrics previousEnemyMetrics = new PlayerMetrics();

	public PlayerAgent(String name, PlayerProperties playerProperties, ISkill[] skills)
	{
		this.player = new Player(name, playerProperties, skills);
	}

	public void Start()
	{
		this.enemy = enemyAgent.GetPLayer();
	}

	public IPlayer GetPLayer()
	{
		return this.player;
	}

	public void SetPosition(Vector3 position)
	{
		this.transform.position = position;
	}

	public void FixedUpdate()
	{
		player.UpdatePlayer(Time.fixedDeltaTime);
	}

	public override void AgentReset()
	{
		player.ResetPlayer();
		battleArena.ResetArea();
	}

	public override void InitializeAgent()
	{
		base.InitializeAgent();
		battleArena = GetComponentInParent<BattleArena>();
		agentRB = GetComponent<Rigidbody>();
	}

	public override void AgentAction(float[] vectorAction, string textAction)
	{
		Debug.Log("AgentAction1 " + vectorAction[0]);
		Debug.Log("AgentAction2 " + vectorAction[0]);

		// Save current metrics before any action
		SaveCurrentMetrics();

		// Make move actions
		MoveAction(Mathf.FloorToInt(vectorAction[0]));

		// Make skill and attack actions
		SkillAction(Mathf.FloorToInt(vectorAction[1]), player, enemy);

		// Give suitable rewards for this state
		GiveRewards();
	}

	public override void CollectObservations()
	{
		// Distance between enemy
		AddVectorObs(Utils.GetDistance(player.GetPosition(), enemy.GetPosition()));

		// Position of player and enemy
		AddVectorObs(player.GetPosition());
		AddVectorObs(enemy.GetPosition());

		// Skill and attack usage information
		AddVectorObs(GetSkillObservations());

		// Stun information
		AddVectorObs(player.IsStunned() ? 1f : 0f);

		// Enemy stun information
		AddVectorObs(enemy.IsStunned() ? 1f : 0f);

		// Health information
		AddVectorObs(player.GetHealth());

		// Buff count
		AddVectorObs(player.GetBuffs().Count);

		// Debuff count
		AddVectorObs(player.GetDebuffs().Count);

		// Enemy debuff count
		AddVectorObs(enemy.GetDebuffs().Count);

		// Set actions masks
		SetActionMask(MOVE_BRANCH_INDEX, Utils.GetMoveMasks(player));
		SetActionMask(SKILL_BRANCH_INDEX, Utils.GetSkillMasks(player, enemy));
	}

	private void SaveCurrentMetrics()
	{
		previousOwnMetrics.Set(player);
		previousEnemyMetrics.Set(enemy);
	}

	private void GiveRewards()
	{
		// player win
		if (enemy.IsDead())
		{
			SetReward(1);
			Done();
		}
		// player loose
		else if (player.IsDead())
		{
			SetReward(-1);
			Done();
		}
		else
		{
			// Give positive reward for applying buff itself
			AddReward(previousOwnMetrics.DiffBuffCount(player) / agentParameters.maxStep);

			// Give positive reward for debuff to enemy
			AddReward(previousEnemyMetrics.DiffDebuffCount(enemy) / agentParameters.maxStep);

			// Give negative reward for applied debuff
			AddReward(-previousOwnMetrics.DiffDebuffCount(player) / agentParameters.maxStep);

			// Give positive or negative reward according to the health difference
			AddReward(previousOwnMetrics.DiffHealth(player) / agentParameters.maxStep);

			// Give bigger negative reward if stunned
			AddReward(previousOwnMetrics.DiffStunned(player) / agentParameters.maxStep * 5);

			// Give bigger positive reward if enemy stunned
			AddReward(-previousEnemyMetrics.DiffStunned(enemy) / agentParameters.maxStep * 5);

			// Tiny negative reward every step
			AddReward(-1 / agentParameters.maxStep);
		}
	}

	private float[] GetSkillObservations()
	{
		ISkill[] skills = player.GetSkills();
		float[] skillObservations = new float[skills.Length + 1];
		for (int i = 0; i < skills.Length; i++)
		{
			skillObservations[i] = skills[i].IsAvailable() ? 0f : 1f;
		}
		// attack observation
		skillObservations[skills.Length] = player.IsAttacking() ? 1f : 0f;
		return skillObservations;
	}

	protected void MoveAction(int action)
	{
		Vector3 direction = Vector3.zero;

		switch (action)
		{
			case 1:
				direction = transform.forward * 1f;
				break;
			case 2:
				direction = transform.forward * -1f;
				break;
			case 3:
				direction = transform.right * 1f;
				break;
			case 4:
				direction = transform.right * -1f;
				break;

		}

		// zero means no move
		if (action != 0)
		{
			transform.Translate(direction);
			player.SetPosition(transform.position);
		}
	}

	public static void SkillAction(int action, IPlayer player, IPlayer enemy)
	{
		// 0 means idle
		// 1 - skill count, means using skill
		if (action > 0 && action < player.GetSkillCount() + 1)
		{
			player.UseSkill(action - 1, enemy);
		}
		// skill count + 1, means normal attack
		else if (action == player.GetSkillCount() + 1)
		{
			player.Attack(enemy);
		}
	}

}
