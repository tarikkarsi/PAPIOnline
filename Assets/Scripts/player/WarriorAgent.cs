public class WarriorAgent : PlayerAgent
{
	static PlayerProperties warriorProps = new PlayerProperties.PlayerPropertiesBuilder()
		   .Experience(0)
		   .Mana(100)
		   .Money(0)
		   .Health(100)
		   .Level(1)
		   .Damage(20)
		   .Speed(1)
		   .HealthPotionCount(10)
		   .ManaPotionCount(10)
		   .Build();

	static AttackSkill skill1 = new AttackSkill(
		"WarriorAttackSkill1",
		10,
		3,
		10,
		30
	);

	static DebuffSkill skill2Debuff = new DebuffSkill(
		"HealthDebuff",
		0,
		10,
		BuffKind.STUN,
		2,
		0,
		false
	);

	static AttackSkill skill2 = new AttackSkill(
		"WarriorAttackSkill2",
		15,
		4,
		15,
		30,
		skill2Debuff,
		100);

	static DebuffSkill skill3Debuff = new DebuffSkill(
		"HealthDebuff",
		0,
		10,
		BuffKind.HEALTH,
		10,
		2,
		true
	);

	static AttackSkill skill3 = new AttackSkill(
		"WarriorAttackSkill3",
		20,
		5,
		20,
		30,
		skill3Debuff,
		10
	);

	static AttackSkill skill4 = new AttackSkill(
		"WarriorAttackSkill4",
		25,
		6,
		25,
		30
	);

	static AttackSkill skill5 = new AttackSkill(
		"WarriorAttackSkill5",
		30,
		7,
		30,
		30
	);

	static BuffSkill skill6 = new BuffSkill(
		"WarriorBuffSkill1",
		20,
		80,
		BuffKind.DAMAGE,
		60,
		10
	);

	static BuffSkill skill7 = new BuffSkill(
		"WarriorBuffSkill2",
		20,
		80,
		BuffKind.SPEED,
		60,
		3
	);

	public WarriorAgent(): base("warrior", warriorProps, new ISkill[] { skill1, skill2, skill3, skill4, skill5, skill6, skill7 })
	{
	}

}
