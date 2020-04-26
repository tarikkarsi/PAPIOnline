using UnityEngine;

namespace PAPIOnline
{

	public class PlayerProperties
	{
		public static int HEALTH_POTION_FILL = 50;
		public static int MANA_POTION_FILL = 25;

		// indicates player's capacity
		public int healthCapacity;

		// indicates player's health
		public int health;

		// indicates player's mana capacity
		public int manaCapacity;

		// indicates player's mana
		public int mana;

		// indicates player's speed
		public float speed;

		// indicates player's defense
		public int defense;

		// indicates player's damage
		public int damage;

		// indicates player's level 
		public int level;

		// indicates player's attack range 
		public int attackRange;

		// indicates player's money
		public int money;

		// indicates player's experience
		public int experience;

		// indicates player's health potion count
		public int healthPotionCount;

		// indicates player's mana potion count
		public int manaPotionCount;

		// indicates player stunned or not
		public bool stunned;

		// indicates player dead or not
		public bool dead;

		// indicates player's position
		public Vector3 position;

		public PlayerProperties Clone()
		{
			PlayerProperties playerProperties = new PlayerProperties();
			playerProperties.healthCapacity = healthCapacity;
			playerProperties.health = health;
			playerProperties.manaCapacity = manaCapacity;
			playerProperties.mana = mana;
			playerProperties.speed = speed;
			playerProperties.damage = damage;
			playerProperties.defense = defense;
			playerProperties.healthPotionCount = healthPotionCount;
			playerProperties.manaPotionCount = manaPotionCount;
			playerProperties.attackRange = attackRange;
			playerProperties.stunned = stunned;
			playerProperties.dead = dead;
			playerProperties.position = position;
			playerProperties.level = level;
			playerProperties.money = money;
			playerProperties.experience = experience;
			return playerProperties;
		}

		public class PlayerPropertiesBuilder
		{
			private int healthCapacity = 0;

			private int health = 0;

			private int manaCapacity = 0;

			private int mana = 0;

			private float speed = 0;

			private int damage = 0;

			private int defense = 0;

			private int attackRange = 0;

			private int healthPotionCount = 0;

			private int manaPotionCount = 0;

			private bool stunned = false;

			private bool dead = false;

			private int level = 0;

			private int money = 0;

			private int experience = 0;

			private Vector3 position = new Vector3();

			public PlayerPropertiesBuilder HealthCapacity(int healthCapacity)
			{
				this.healthCapacity = healthCapacity;
				return this;
			}

			public PlayerPropertiesBuilder Health(int health)
			{
				this.health = health;
				return this;
			}

			public PlayerPropertiesBuilder ManaCapacity(int manaCapacity)
			{
				this.manaCapacity = manaCapacity;
				return this;
			}

			public PlayerPropertiesBuilder Mana(int mana)
			{
				this.mana = mana;
				return this;
			}

			public PlayerPropertiesBuilder Speed(float speed)
			{
				this.speed = speed;
				return this;
			}

			public PlayerPropertiesBuilder Damage(int damage)
			{
				this.damage = damage;
				return this;
			}

			public PlayerPropertiesBuilder Defense(int defense)
			{
				this.defense = defense;
				return this;
			}

			public PlayerPropertiesBuilder AttackRange(int attackRange)
			{
				this.attackRange = attackRange;
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

			public PlayerPropertiesBuilder Level(int level)
			{
				this.level = level;
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

			public PlayerProperties Build()
			{
				PlayerProperties playerProperties = new PlayerProperties();
				playerProperties.healthCapacity = healthCapacity;
				playerProperties.health = health;
				playerProperties.manaCapacity = manaCapacity;
				playerProperties.mana = mana;
				playerProperties.speed = speed;
				playerProperties.damage = damage;
				playerProperties.defense = defense;
				playerProperties.healthPotionCount = healthPotionCount;
				playerProperties.manaPotionCount = manaPotionCount;
				playerProperties.attackRange = attackRange;
				playerProperties.stunned = stunned;
				playerProperties.dead = dead;
				playerProperties.position = position;
				playerProperties.level = level;
				playerProperties.money = money;
				playerProperties.experience = experience;
				return playerProperties;
			}

		}

	}

}