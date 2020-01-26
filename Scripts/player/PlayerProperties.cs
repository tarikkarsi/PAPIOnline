using UnityEngine;

public class PlayerProperties
{
	public static float HEALTH_POTION_FILL = 25;

	public static int MANA_POTION_FILL = 25;

	// indicates character’s health
	public float health;

	// indicates character’s speed
	public float speed;

	// indicates character’s defense
	public float defense;

	// indicates character’s damage
	public float damage;

	// indicates character’s level 
	public int level;

	// indicates character’s attack range 
	public int attackRange;

	// indicates character’s money
	public int money;

	// indicates character’s experience
	public float experience;

	// indicates character’s mana
	public int mana;

	// indicates character’s health potion count
	public int healthPotionCount;

	// indicates character’s mana potion count
	public int manaPotionCount;

	// indicates character stunned or not
	public bool stunned;

	// indicates character dead or not
	public bool dead;

	// indicates character’s position
	public Vector3 position;

	public PlayerProperties Clone()
	{
		PlayerProperties playerProperties = new PlayerProperties();
		playerProperties.health = health;
		playerProperties.speed = speed;
		playerProperties.damage = damage;
		playerProperties.defense = defense;
		playerProperties.level = level;
		playerProperties.money = money;
		playerProperties.mana = mana;
		playerProperties.experience = experience;
		playerProperties.healthPotionCount = healthPotionCount;
		playerProperties.manaPotionCount = manaPotionCount;
		playerProperties.attackRange = attackRange;
		playerProperties.stunned = stunned;
		playerProperties.dead = dead;
		playerProperties.position = position;
		return playerProperties;
	}

	public class PlayerPropertiesBuilder
	{
		public float health = 0;

		public float speed = 0;

		public float damage = 0;

		public float defense = 0;

		public int level = 0;

		public int attackRange = 0;

		public int money = 0;

		public float experience = 0;

		public int mana = 0;

		public int healthPotionCount = 0;

		public int manaPotionCount = 0;

		public bool stunned = false;

		public bool dead = false;

		public Vector3 position = new Vector3();

		public PlayerPropertiesBuilder Health(float health)
		{
			this.health = health;
			return this;
		}

		public PlayerPropertiesBuilder Level(int level)
		{
			this.level = level;
			return this;
		}

		public PlayerPropertiesBuilder Speed(float speed)
		{
			this.speed = speed;
			return this;
		}

		public PlayerPropertiesBuilder Damage(float damage)
		{
			this.damage = damage;
			return this;
		}

		public PlayerPropertiesBuilder Defense(float defense)
		{
			this.defense = defense;
			return this;
		}

		public PlayerPropertiesBuilder Money(int money)
		{
			this.money = money;
			return this;
		}

		public PlayerPropertiesBuilder Experience(int experience)
		{
			this.experience = experience;
			return this;
		}

		public PlayerPropertiesBuilder Mana(int mana)
		{
			this.mana = mana;
			return this;
		}

		public PlayerPropertiesBuilder HealthPotionCount(int healthPotionCount)
		{
			this.healthPotionCount = healthPotionCount;
			return this;
		}

		public PlayerPropertiesBuilder ManaPotionCount(int manaPotionCount)
		{
			this.manaPotionCount = manaPotionCount;
			return this;
		}

		public PlayerProperties Build()
		{
			PlayerProperties playerProperties = new PlayerProperties();
			playerProperties.health = health;
			playerProperties.speed = speed;
			playerProperties.damage = damage;
			playerProperties.level = level;
			playerProperties.money = money;
			playerProperties.mana = mana;
			playerProperties.experience = experience;
			playerProperties.healthPotionCount = healthPotionCount;
			playerProperties.manaPotionCount = manaPotionCount;
			playerProperties.attackRange = attackRange;
			playerProperties.stunned = stunned;
			playerProperties.dead = dead;
			playerProperties.position = position;
			return playerProperties;
		}

	}

}
