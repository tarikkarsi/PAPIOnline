/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          Player
 *   
 *   Description:    Player implementation
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PAPIOnline
{

	public class Player : IPlayer
	{
		private static float SKILL_ANIMATION_DURATION = 0.4f;
		private static float ATTACK_ANIMATION_DURATION = 0.4f;

		protected string name;

		protected PlayerProperties properties;
		protected PlayerProperties backupProperties;

		protected ISkill[] skills;
		protected List<IBuffSkill> appliedBuffs;
		protected List<IBuffSkill> appliedDebuffs;

		// Timers
		protected float attackAnimationTimer;
		protected float skillAnimationTimer;

		protected CollisionManager collisionManager;

		public Player(String name, PlayerProperties playerProperties, ISkill[] skills)
		{
			this.name = name;
			this.properties = playerProperties;
			this.backupProperties = playerProperties.Clone();
			this.skills = skills;
			this.appliedBuffs = new List<IBuffSkill>();
			this.appliedDebuffs = new List<IBuffSkill>();
			this.attackAnimationTimer = 0;
			this.skillAnimationTimer = 0;
		}

		private Player()
		{
			// Empty constructor is only for cloning purposes
		}

		public void ResetPlayer()
		{
			this.properties = this.backupProperties.Clone();
			this.appliedBuffs.Clear();
			this.appliedDebuffs.Clear();
			foreach (ISkill skill in this.skills)
			{
				skill.ResetSkill();
			}
			this.attackAnimationTimer = 0;
			this.skillAnimationTimer = 0;
		}

		public string GetName()
		{
			return this.name;
		}

		public void SetName(string name)
		{
			this.name = name;
		}

		public PlayerProperties GetProperties()
		{
			return this.properties;
		}

		public Vector3 GetPosition()
		{
			return this.properties.position;
		}

		public void SetPosition(Vector3 position)
		{
			this.properties.position.x = position.x;
			this.properties.position.y = position.y;
			this.properties.position.z = position.z;
		}

		public void IncreaseSpeed(float amount)
		{
			this.properties.speed += amount;
		}

		public void DecreaseSpeed(float amount)
		{
			this.properties.speed -= amount;
		}

		public float GetSpeed()
		{
			return this.properties.speed;
		}

		public int GetHealthCapacity()
		{
			return this.properties.healthCapacity;
		}

		public int GetHealth()
		{
			return this.properties.health;
		}

		public void IncreaseHealth(int amount)
		{
			this.properties.health += amount;
		}

		public void DecreaseHealth(int amount)
		{
			this.properties.health -= amount;
			if (this.properties.health <= 0)
			{
				this.SetDead(true);
			}
		}

		public int GetManaCapacity()
		{
			return this.properties.manaCapacity;
		}

		public int GetMana()
		{
			return this.properties.mana;
		}

		public void DecreaseMana(int amount)
		{
			this.properties.mana -= amount;
		}

		public void IncreaseMana(int amount)
		{
			this.properties.mana += amount;
		}

		public int GetDamage()
		{
			return this.properties.damage;
		}

		public void IncreaseDamage(int amount)
		{
			this.properties.damage += amount;
		}

		public void DecreaseDamage(int amount)
		{
			this.properties.damage -= amount;
		}

		public int GetDefense()
		{
			return this.properties.defense;
		}

		public void IncreaseDefense(int amount)
		{
			this.properties.defense += amount;
		}

		public void DecreaseDefense(int amount)
		{
			this.properties.defense -= amount;
		}

		public int GetLevel()
		{
			return this.properties.level;
		}

		public void IncreaseLevel()
		{
			this.properties.level++;
		}

		public int GetAttackRange()
		{
			return this.properties.attackRange;
		}

		public IList<IBuffSkill> GetAppliedBuffs()
		{
			return this.appliedBuffs;
		}

		public void AddAppliedBuff(IBuffSkill buff)
		{
			this.appliedBuffs.Add(buff);
		}

		public void RemoveAppliedBuff(IBuffSkill buff)
		{
			this.appliedBuffs.Remove(buff);
		}

		public IList<IBuffSkill> GetAppliedDebuffs()
		{
			return this.appliedDebuffs;
		}

		public void AddAppliedDebuff(IBuffSkill debuff)
		{
			this.appliedDebuffs.Add(debuff);
		}

		public void RemoveAppliedDebuff(IBuffSkill debuff)
		{
			this.appliedDebuffs.Remove(debuff);
		}

		public int GetHealthPotionCount()
		{
			return this.properties.healthPotionCount;
		}

		public int GetManaPotionCount()
		{
			return this.properties.manaPotionCount;
		}

		public int GetExperience()
		{
			return this.properties.experience;
		}

		public void IncreaseExperience(int amount)
		{
			this.properties.experience += amount;
			if (this.properties.experience >= 100)
			{
				// Level-up
				this.IncreaseLevel();
				this.properties.experience = 0;
			}
		}

		public void DecreaseExperience(int amount)
		{
			this.properties.experience -= amount;
			if (this.properties.experience < 0)
			{
				this.properties.experience = 0;
			}
		}

		public int GetMoney()
		{
			return this.properties.money;
		}

		public void IncreaseMoney(int amount)
		{
			this.properties.money += amount;
		}

		public bool Move(Vector3 direction)
		{
			if (this.IsAvailable())
			{
				// Do not allow changing y direction
				direction.y = 0;
				Vector3 dirToGo = direction * this.GetSpeed();
				// Check collision of other object if collision manager exists
				if (this.collisionManager == null || !this.collisionManager.WillPlayerCollide(this, dirToGo)) {
					this.properties.position += dirToGo;
					return true;
				}
			}
			else
			{
				Debug.LogError(GetName() + " is not available (Player::Move)");
			}
			return false;
		}

		public bool Attack(IPlayer target)
		{
			// Check availability and distance
			if (this.IsAvailable() && Utils.CanAttack(this, target))
			{
				target.DecreaseHealth(this.GetDamage() - target.GetDefense());
				this.attackAnimationTimer = ATTACK_ANIMATION_DURATION;
				//Debug.LogError(GetName() + " is attacked to enemy " + (this.GetDamage() - target.GetDefense()));
				return true;
			}
			else
			{
				if (!this.IsAvailable())
				{
					Debug.LogError(GetName() + " is not available (Player::Attack)");
				}
				else
				{
					Debug.LogError(GetName() + " is not close enough (Player::Attack)");
				}
			}
			return false;
		}

		public bool UseHealthPotion()
		{
			// Check health potion count and health capacity
			if (this.properties.healthPotionCount > 0 && this.properties.health < this.properties.healthCapacity)
			{
				int fillAmount = this.properties.healthCapacity - this.properties.health;

				if (fillAmount > PlayerProperties.HEALTH_POTION_FILL)
				{
					fillAmount = PlayerProperties.HEALTH_POTION_FILL;
				}

				this.properties.health += fillAmount;
				this.properties.healthPotionCount--;
				// Debug.Log(GetName() + " used health potion and filled " + fillAmount + " of health (Player::UseHealthPotion)");
				return true;
			}
			else
			{
				if (this.properties.healthPotionCount == 0)
				{
					Debug.LogError(GetName() + " has not enough health potion (Player::UseHealthPotion)");
				}
				else
				{
					Debug.LogError(GetName() + " health already full (Player::UseHealthPotion)");
				}
			}
			return false;
		}

		public bool UseManaPotion()
		{
			// Check mana potion count and mana capacity
			if (this.properties.manaPotionCount > 0 && this.properties.mana < this.properties.manaCapacity)
			{
				int fillAmount = this.properties.manaCapacity - this.properties.mana;

				if (fillAmount > PlayerProperties.MANA_POTION_FILL)
				{
					fillAmount = PlayerProperties.MANA_POTION_FILL;
				}

				this.properties.mana += fillAmount;
				this.properties.manaPotionCount--;
				// Debug.Log(GetName() + " used mana potion and filled " + fillAmount + " of mana (Player::UseManaPotion)");
				return true;
			}
			else
			{
				if (this.properties.manaPotionCount == 0)
				{
					Debug.LogError(GetName() + " has not enough mana potion (Player::UseManaPotion) manaPotionCount: " + this.properties.manaPotionCount);
				}
				else
				{
					Debug.LogError(GetName() + " mana already full (Player::UseManaPotion) mana: " + this.properties.mana);
				}
			}
			return false;
		}

		public bool UseSkill(int skillIndex, IPlayer target)
		{
			// Use skill on target enemy
			ISkill skill = this.skills[skillIndex];
			// Check for availability
			if (this.IsAvailable())
			{
				bool success = this.skills[skillIndex].Use(this, target);
				if (success)
				{
					this.skillAnimationTimer = SKILL_ANIMATION_DURATION;
					// Debug.Log(GetName() + " used skill with index: " + skillIndex + "(Player::UseSkill)");
					return true;
				}
				else
				{
					Debug.LogError(GetName() + " skill with index: " + skillIndex + " is not available (Player::UseSkill)");
				}
			}
			else
			{
				Debug.LogError(GetName() + " is not available to use skill with index: " + skillIndex + " (Player::UseSkill) SkillAnimationTimer: " + this.skillAnimationTimer);
			}
			return false;
		}

		public ISkill[] GetSkills()
		{
			return this.skills;
		}

		public void SetSkills(ISkill[] skills)
		{
			this.skills = skills;
		}

		public int GetSkillCount()
		{
			return this.skills.Length;
		}

		public void UpdatePlayer(float elapsedTime)
		{
			// Update timers
			this.attackAnimationTimer -= elapsedTime;
			this.skillAnimationTimer -= elapsedTime;

			// Update skills
			foreach (ISkill skill in this.skills)
			{
				skill.Update(elapsedTime);
			}

			// Update applied buffs
			// Iterate with for and reverse because timeouted buffs will be removed in UpdateBuff
			for (int i = this.appliedBuffs.Count - 1; i >= 0; i--)
			{
				this.appliedBuffs[i].UpdateBuff(this, elapsedTime);
			}

			// Update applied debuffs
			// Iterate with for and reverse because timeouted debuffs will be removed in UpdateBuff
			for (int i = this.appliedDebuffs.Count - 1; i >= 0; i--)
			{
				this.appliedDebuffs[i].UpdateBuff(this, elapsedTime);
			}
		}

		public void SetStunned(bool stunned)
		{
			this.properties.stunned = stunned;
		}

		public bool IsStunned()
		{
			return this.properties.stunned;
		}

		public void SetDead(bool dead)
		{
			this.properties.dead = dead;
		}

		public bool IsDead()
		{
			return this.properties.dead;
		}

		public bool IsAttacking()
		{
			return this.attackAnimationTimer > 0.0f;
		}

		public bool IsUsingSkill()
		{
			return this.skillAnimationTimer > 0.0f;
		}

		public bool IsAvailable()
		{
			return !this.IsStunned() && !this.IsUsingSkill() && !this.IsAttacking() && !this.IsDead();
		}

		public void SetCollisionManager(CollisionManager collisionManager)
		{
			this.collisionManager = collisionManager.RegisterPlayer(this);
		}

		public IPlayer ClonePlayer()
		{
			Player clone = new Player();
			clone.name = name;
			// Clone properties
			clone.properties = this.properties.Clone();
			// Clone skills
			clone.skills = new ISkill[this.skills.Length];
			for (int i = 0; i < this.skills.Length; i++)
			{
				clone.skills[i] = this.skills[i].CloneSkill();
			}
			// Clone buffs
			clone.appliedBuffs = new List<IBuffSkill>();
			foreach (IBuffSkill buff in this.appliedBuffs)
			{
				clone.appliedBuffs.Add(buff.CloneBuffSkill());
			}
			// Clone debuffs
			clone.appliedDebuffs = new List<IBuffSkill>();
			foreach (IBuffSkill debuff in this.appliedDebuffs)
			{
				clone.appliedDebuffs.Add(debuff.CloneBuffSkill());
			}
			// Clone other fields
			clone.attackAnimationTimer = this.attackAnimationTimer;
			clone.skillAnimationTimer = this.skillAnimationTimer;
			return clone;
		}
	}

}