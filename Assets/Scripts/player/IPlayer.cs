using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public interface IPlayer
	{
		// getter for player's name
		string GetName();

		// setter for player's name
		void SetName(string name);

		// getter for player's health capacity
		int GetHealthCapacity();

		// getter for player's health
		int GetHealth();

		// getter for player's mana capacity
		int GetManaCapacity();

		// getter for player's mana
		int GetMana();

		// increase player's mana by given amount
		void IncreaseMana(int amount);

		// decrease player's mana by given amount
		void DecreaseMana(int amount);

		// increase player's health by the given amount
		void IncreaseHealth(int amount);

		// decrease player's health by the given amount
		void DecreaseHealth(int amount);

		// getter for player's speed
		float GetSpeed();

		// increase player's speed by the given amount
		void IncreaseSpeed(float amount);

		// decrease player's speed by the given amount
		void DecreaseSpeed(float amount);

		// getter for player's damage
		int GetDamage();

		// increase player's damage by the given amount
		void IncreaseDamage(int amount);

		// increase player's damage by the given amount
		void DecreaseDamage(int amount);

		// getter for player's defense
		int GetDefense();

		// increase player's defense by the given amount
		void IncreaseDefense(int amount);

		// increase player's defense by the given amount
		void DecreaseDefense(int amount);

		// getter for player's level
		int GetLevel();

		// increase player's level
		void IncreaseLevel();

		// getter for player's position
		Vector3 GetPosition();

		// setter for player's position
		void SetPosition(Vector3 position);

		// getter for player's attack range
		int GetAttackRange();

		// makes and attack to given target
		void Attack(IPlayer target);

		// getter for player's attack animation
		bool IsAttacking();

		// moves player to given direction
		void Move(Vector3 direction);

		// getter for applied buffs on the player
		IList<IBuffSkill> GetAppliedBuffs();

		// add a buff effect to the player
		void AddAppliedBuff(IBuffSkill buff);

		// remove previously added buff effect from the player
		void RemoveAppliedBuff(IBuffSkill buff);

		// getter for applied debuffs on the player
		IList<IBuffSkill> GetAppliedDebuffs();

		// add a debuff effect to the player
		void AddAppliedDebuff(IBuffSkill debuff);

		// remove previously added debuff effect from the player
		void RemoveAppliedDebuff(IBuffSkill debuff);

		// getter for player's experience
		int GetExperience();

		// increase player's experience by given amount
		void IncreaseExperience(int amount);

		// decrease player's experience by given amount
		void DecreaseExperience(int amount);

		// getter for player's money
		int GetMoney();

		// increase player's money by given amount
		void IncreaseMoney(int amount);

		// getter for player's health potion count
		int GetHealthPotionCount();

		// use health potion
		void UseHealthPotion();

		// getter for player's mana potion count
		int GetManaPotionCount();

		// use mana potion
		void UseManaPotion();

		// getter for player's skills
		ISkill[] GetSkills();

		// setter for player's skills
		void SetSkills(ISkill[] skills);

		// getter for player's skill count
		int GetSkillCount();

		// use the skill with given index
		void UseSkill(int skillIndex, IPlayer target);

		// getter for player's skill animation
		bool IsUsingSkill();

		// setter for player's stun information
		void SetStunned(bool stunned);

		// getter for player's stun information
		bool IsStunned();

		// setter for player's dead information
		void SetDead(bool dead);

		// getter for player's dead information
		bool IsDead();

		// creates clone of player
		IPlayer ClonePlayer();

		// resets player properties
		void ResetPlayer();

		// getter for player's available information. !this.IsStunned() && !this.IsUsingSkill() && !this.IsAttacking() && !this.IsDead();
		bool IsAvailable();

		// updates player's timers, skills and position
		void UpdatePlayer(float elapsedTime);

	}

}