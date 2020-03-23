using System;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class PlayerAgent : Agent
{
	private static int MOVE_BRANCH_INDEX = 0;
	private static int SKILL_BRANCH_INDEX = 1;

	private IPlayer player;
	private IPlayer enemy;

	private Rigidbody agentRB;

	private PlayerMetrics previousOwnMetrics = new PlayerMetrics();
	private PlayerMetrics previousEnemyMetrics = new PlayerMetrics();

	public PlayerAgent(String name, PlayerProperties playerProperties, ISkill[] skills)
	{
		this.player = new Player(name, playerProperties, skills);
	}

	public void Start()
	{
		BattleArena arena = GetComponentInParent<BattleArena>();
		this.enemy = arena.getRival(tag).GetPLayer();
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

	public override void OnEpisodeBegin()
	{
		player.ResetPlayer();
	}

	public override void Initialize()
	{
		base.Initialize();
		agentRB = GetComponent<Rigidbody>();
	}

	public override void OnActionReceived(float[] vectorAction)
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

	public override void CollectObservations(VectorSensor sensor)
	{
		// Distance between enemy
		sensor.AddObservation(Utils.GetDistance(player.GetPosition(), enemy.GetPosition()));

		// Position of player and enemy
		sensor.AddObservation(player.GetPosition());
		sensor.AddObservation(enemy.GetPosition());

		// Skill and attack usage information
		sensor.AddObservation(GetSkillObservations());

		// Stun information
		sensor.AddObservation(player.IsStunned() ? 1f : 0f);

		// Enemy stun information
		sensor.AddObservation(enemy.IsStunned() ? 1f : 0f);

		// Health information
		sensor.AddObservation(player.GetHealth());

		// Buff count
		sensor.AddObservation(player.GetBuffs().Count);

		// Debuff count
		sensor.AddObservation(player.GetDebuffs().Count);

		// Enemy debuff count
		sensor.AddObservation(enemy.GetDebuffs().Count);
	}

	public override void CollectDiscreteActionMasks(DiscreteActionMasker actionMasker)
	{
		// Set actions masks
		actionMasker.SetMask(MOVE_BRANCH_INDEX, Utils.GetMoveMasks(player));
		actionMasker.SetMask(SKILL_BRANCH_INDEX, Utils.GetSkillMasks(player, enemy));
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
			EndEpisode();
		}
		// player loose
		else if (player.IsDead())
		{
			SetReward(-1);
			EndEpisode();
		}
		else
		{
			// Give positive reward for applying buff itself
			AddReward(previousOwnMetrics.DiffBuffCount(player) / maxStep);

			// Give positive reward for debuff to enemy
			AddReward(previousEnemyMetrics.DiffDebuffCount(enemy) / maxStep);

			// Give negative reward for applied debuff
			AddReward(-previousOwnMetrics.DiffDebuffCount(player) / maxStep);

			// Give positive or negative reward according to the health difference
			AddReward(previousOwnMetrics.DiffHealth(player) / maxStep);

			// Give bigger negative reward if stunned
			AddReward(previousOwnMetrics.DiffStunned(player) / maxStep * 5);

			// Give bigger positive reward if enemy stunned
			AddReward(-previousEnemyMetrics.DiffStunned(enemy) / maxStep * 5);

			// Tiny negative reward every step
			AddReward(-1 / maxStep);
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
