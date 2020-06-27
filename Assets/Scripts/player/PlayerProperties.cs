/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          PlayerProperties
 *   
 *   Description:    Player property contains common player character features
 *					 in MMORPGs. It allows creating a character with the desired
 *					 feature. Every player character created in our game should
 *					 have these features. Players have physical and situational
 *					 properties. Physical properties such as health, defense,
 *					 speed, and damage contribute to the player's battle with
 *					 other players and monsters. Situational properties such as
 *					 stun, dead, experience and level determine the situation
 *					 the player is in during and after these battles.
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;

namespace PAPIOnline
{

	public class PlayerProperties
	{
		public static int HEALTH_POTION_FILL = 50;

		public static int MANA_POTION_FILL = 25;

		public static int MAX_HEALTH = 1000;

		public static int MAX_MANA = 1000;

		public static int MAX_SPEED = 30;

		public static int MAX_DEFENSE = 25;

		public static int MAX_DAMAGE = 20;

		public static int MAX_MANA_POTION = 10;

		public static int MAX_HEALTH_POTION = 10;

		public static int MAX_ATTACK_RANGE = 4;

		// Indicates player's capacity
		public int healthCapacity;

		// Indicates player's health
		public int health;

		// Indicates player's mana capacity
		public int manaCapacity;

		// Indicates player's mana
		public int mana;

		// Indicates player's speed
		public float speed;

		// Indicates player's defense
		public int defense;

		// Indicates player's damage
		public int damage;

		// Indicates player's level 
		public int level;

		// Indicates player's attack range 
		public int attackRange;

		// Indicates player's money
		public int money;

		// Indicates player's experience
		public int experience;

		// Indicates player's health potion count
		public int healthPotionCount;

		// Indicates player's mana potion count
		public int manaPotionCount;

		// Indicates player stunned or not
		public bool stunned;

		// Indicates player dead or not
		public bool dead;

		// Indicates player's position
		public Vector3 position = new Vector3();

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
			playerProperties.position.x = position.x;
			playerProperties.position.y = position.y;
			playerProperties.position.z = position.z;
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
				playerProperties.position.x = position.x;
				playerProperties.position.y = position.y;
				playerProperties.position.z = position.z;
				playerProperties.level = level;
				playerProperties.money = money;
				playerProperties.experience = experience;
				return playerProperties;
			}

		}

	}

}