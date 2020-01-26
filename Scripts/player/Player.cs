using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : IPlayer
{
	protected string characterName;

	protected PlayerProperties playerProperties;
	protected PlayerProperties backupProperties;

	protected ISkill[] skills;
	protected List<IBuffSkill> buffs;
	protected List<IBuffSkill> debuffs;

	// Timers
	protected float attackAnimationTimer;
	protected float skillAnimationTimer;

	public Player(String name, PlayerProperties playerProperties, ISkill[] skills)
	{
		this.characterName = name;
		this.playerProperties = playerProperties;
		this.backupProperties = playerProperties.Clone();
		this.skills = skills;
		this.buffs = new List<IBuffSkill>();
		this.debuffs = new List<IBuffSkill>();
		this.attackAnimationTimer = 0;
		this.skillAnimationTimer = 0;
	}

	private Player()
	{
		// Empty constructor is only for cloning purposes
	}

	public void ResetPlayer()
	{
		playerProperties = backupProperties.Clone();
		buffs.Clear();
		debuffs.Clear();
		foreach (ISkill skill in skills)
		{
			skill.ResetSkill();
		}
	}

	public string GetName()
	{
		return this.characterName;
	}

	public void SetName(string name)
	{
		this.characterName = name;
	}

	public Vector3 GetPosition()
	{
		return this.playerProperties.position;
	}

	public void SetPosition(Vector3 position)
	{
		this.playerProperties.position = position;
	}

	public void IncreaseSpeed(float amount)
	{
		this.playerProperties.speed += amount;
	}

	public void DecreaseSpeed(float amount)
	{
		this.playerProperties.speed -= amount;
	}

	public float GetSpeed()
	{
		return this.playerProperties.speed;
	}

	public float GetHealth()
	{
		return this.playerProperties.health;
	}

	public void IncreaseHealth(float amount)
	{
		this.playerProperties.health += amount;
	}

	public void DecreaseHealth(float amount)
	{
		this.playerProperties.health -= amount;
		if (this.playerProperties.health <= 0)
		{
			this.SetDead(true);
		}
	}

	public float GetDamage()
	{
		return this.playerProperties.damage;
	}

	public void IncreaseDamage(float amount)
	{
		this.playerProperties.damage += amount;
	}

	public void DecreaseDamage(float amount)
	{
		this.playerProperties.damage -= amount;
	}

	public float GetDefense()
	{
		return this.playerProperties.defense;
	}

	public void IncreaseDefense(float amount)
	{
		this.playerProperties.defense += amount;
	}

	public void DecreaseDefense(float amount)
	{
		this.playerProperties.defense -= amount;
	}

	public int GetLevel()
	{
		return this.playerProperties.level;
	}

	public void IncreaseLevel()
	{
		this.playerProperties.level++;
	}

	public int GetAttackRange()
	{
		return this.playerProperties.attackRange;
	}

	public IList<IBuffSkill> GetBuffs()
	{
		return this.buffs;
	}

	public void AddBuff(IBuffSkill buff)
	{
		this.buffs.Add(buff);
	}

	public void RemoveBuff(IBuffSkill buff)
	{
		this.buffs.Remove(buff);
	}

	public IList<IBuffSkill> GetDebuffs()
	{
		return this.debuffs;
	}

	public void AddDebuff(IBuffSkill debuff)
	{
		this.debuffs.Add(debuff);
	}

	public void RemoveDebuff(IBuffSkill debuff)
	{
		this.debuffs.Remove(debuff);
	}

	public int GetHealthPotionCount()
	{
		return this.playerProperties.healthPotionCount;
	}

	public int GetManaPotionCount()
	{
		return this.playerProperties.manaPotionCount;
	}

	public int GetMana()
	{
		return this.playerProperties.mana;
	}

	public void DecreaseMana(int amount)
	{
		this.playerProperties.mana -= amount;
	}

	public void IncreaseMana(int amount)
	{
		this.playerProperties.mana += amount;
	}

	public float GetExperience()
	{
		return this.playerProperties.experience;
	}

	public void IncreaseExperience(float amount)
	{
		this.playerProperties.experience += amount;
		if (this.playerProperties.experience >= 100)
		{
			// level up
			this.IncreaseLevel();
			this.playerProperties.experience = 0;
		}
	}

	public void DecreaseExperience(float amount)
	{
		this.playerProperties.experience -= amount;
		if (this.playerProperties.experience < 0)
		{
			this.playerProperties.experience = 0;
		}
	}

	public int GetMoney()
	{
		return this.playerProperties.money;
	}

	public void IncreaseMoney(int amount)
	{
		this.playerProperties.money += amount;
	}

	public void Move(Vector3 direction)
	{
		this.playerProperties.position += direction * this.GetSpeed();
	}

	public void Attack(IPlayer target)
	{
		if (!this.IsAvailable() && Utils.GetDistance(GetPosition(), target.GetPosition()) <= GetAttackRange())
		{
			target.DecreaseHealth(this.GetDamage());
			this.attackAnimationTimer = 1;
		}
	}

	public void UseHealthPotion()
	{
		Debug.Log("Use health potion");
		// check health potion count
		if (this.playerProperties.healthPotionCount > 0)
		{
			float previousHealth = this.playerProperties.health;
			this.playerProperties.health += PlayerProperties.HEALTH_POTION_FILL;
			// health can not be bigger than 100
			if (this.playerProperties.health > 100)
			{
				this.playerProperties.health = 100;
			}
			// check for usage
			if (!this.playerProperties.health.Equals(previousHealth))
			{
				this.playerProperties.healthPotionCount--;
				Debug.Log("Health potion count: " + this.playerProperties.healthPotionCount);
			}
			else
			{
				Debug.Log("Health already full");
			}
		}
		else
		{
			Debug.Log("Not enough health potion");
		}
	}

	public void UseManaPotion()
	{
		Debug.Log("Use mana potion");
		// check mana potion count
		if (this.playerProperties.manaPotionCount > 0)
		{
			int previousMana = this.playerProperties.mana;
			this.playerProperties.mana += PlayerProperties.MANA_POTION_FILL;
			// mana can not be bigger than 100
			if (this.playerProperties.mana > 100)
			{
				this.playerProperties.mana = 100;
			}
			// check for usage
			if (this.playerProperties.mana != previousMana)
			{
				this.playerProperties.manaPotionCount--;
				Debug.Log("Mana potion count: " + this.playerProperties.manaPotionCount);
			}
			else
			{
				Debug.Log("Mana already full");
			}
		}
		else
		{
			Debug.Log("Not enough mana potion");
		}
	}

	public void UseSkill(int skillIndex, IPlayer target)
	{
		Debug.Log("Use skill with index: " + skillIndex);
		// use skill on target enemy
		if (!this.IsAvailable())
		{
			bool success = this.skills[skillIndex].Use(this, target);
			if (success)
			{
				this.skillAnimationTimer = 1f;
			}
		}
		else
		{
			Debug.Log("Use skill skill animation is not finished");
		}
	}

	public ISkill[] GetSkills()
	{
		return this.skills;
	}

	public int GetSkillCount()
	{
		return this.skills.Length;
	}

	public void UpdatePlayer(float elapsedTime)
	{
		// update timers
		this.attackAnimationTimer -= elapsedTime;
		this.skillAnimationTimer -= elapsedTime;

		// update skills
		foreach (ISkill skill in this.skills)
		{
			skill.Update(elapsedTime);
		}

		// update applied buffs/debuffs
		foreach (IBuffSkill appliedBuff in this.buffs)
		{
			appliedBuff.UpdateBuff(this, elapsedTime);
		}
	}

	public void SetStunned(bool stunned)
	{
		this.playerProperties.stunned = stunned;
	}

	public bool IsStunned()
	{
		return this.playerProperties.stunned;
	}

	public void SetDead(bool dead)
	{
		this.playerProperties.dead = dead;
	}

	public bool IsDead()
	{
		return this.playerProperties.dead;
	}

	public bool IsAttacking()
	{
		return this.attackAnimationTimer > 0;
	}

	public bool IsUsingSkill()
	{
		return this.skillAnimationTimer > 0;
	}

	public bool IsAvailable()
	{
		return this.IsStunned() || this.IsDead() || this.IsUsingSkill() || this.IsAttacking();
	}

	public IPlayer ClonePlayer()
	{
		Player clone = new Player();
		clone.characterName = characterName;
		// clone properties
		clone.playerProperties = this.playerProperties.Clone();
		// clone skills
		clone.skills = new ISkill[this.skills.Length];
		for (int i = 0; i < this.skills.Length; i++)
		{
			clone.skills[i] = this.skills[i].CloneSkill();
		}
		// clone buffs
		clone.buffs = new List<IBuffSkill>();
		foreach (IBuffSkill buff in this.buffs)
		{
			clone.buffs.Add(buff.CloneBuffSkill());
		}
		// clone debuffs
		clone.debuffs = new List<IBuffSkill>();
		foreach (IBuffSkill debuff in this.debuffs)
		{
			clone.debuffs.Add(debuff.CloneBuffSkill());
		}
		// clone other fields
		clone.attackAnimationTimer = this.attackAnimationTimer;
		clone.skillAnimationTimer = this.skillAnimationTimer;
		return clone;
	}
}
