/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          IPlayer
 *   
 *   Description:    The behavior and actions of a player character are
 *					 determined through the player interface which includes
 *					 common player character behavior in MMORPGs. Each player
 *					 character created in our game must implement this
 *					 interface. The properties of the players may vary during
 *					 the game. They are constantly in motion on the game world.
 *					 They can use their skills to attack. They can gain
 *					 experience and level up.
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace PAPIOnline
{

	public interface IPlayer
	{
		// Getter for player's name
		string GetName();

		// Setter for player's name
		void SetName(string name);

		// Getter for player's properties
		PlayerProperties GetProperties();

		// Getter for player's health capacity
		int GetHealthCapacity();

		// Getter for player's health
		int GetHealth();

		// Getter for player's mana capacity
		int GetManaCapacity();

		// Getter for player's mana
		int GetMana();

		// Increase player's mana by given amount
		void IncreaseMana(int amount);

		// Decrease player's mana by given amount
		void DecreaseMana(int amount);

		// Increase player's health by the given amount
		void IncreaseHealth(int amount);

		// Decrease player's health by the given amount
		void DecreaseHealth(int amount);

		// Getter for player's speed
		float GetSpeed();

		// Increase player's speed by the given amount
		void IncreaseSpeed(float amount);

		// Decrease player's speed by the given amount
		void DecreaseSpeed(float amount);

		// Getter for player's damage
		int GetDamage();

		// Increase player's damage by the given amount
		void IncreaseDamage(int amount);

		// Decrease player's damage by the given amount
		void DecreaseDamage(int amount);

		// Getter for player's defense
		int GetDefense();

		// Increase player's defense by the given amount
		void IncreaseDefense(int amount);

		// Increase player's defense by the given amount
		void DecreaseDefense(int amount);

		// Getter for player's level
		int GetLevel();

		// Increase player's level
		void IncreaseLevel();

		// Getter for player's position
		Vector3 GetPosition();

		// Setter for player's position
		void SetPosition(Vector3 position);

		// Getter for player's attack range
		int GetAttackRange();

		// Makes and attack to given target
		bool Attack(IPlayer target);

		// Getter for player's attack animation
		bool IsAttacking();

		// Moves player to given direction, returns false if collided
		bool Move(Vector3 direction);

		// Getter for applied buffs on the player
		IList<IBuffSkill> GetAppliedBuffs();

		// Add a buff effect to the player
		void AddAppliedBuff(IBuffSkill buff);

		// Remove previously added buff effect from the player
		void RemoveAppliedBuff(IBuffSkill buff);

		// Getter for applied debuffs on the player
		IList<IBuffSkill> GetAppliedDebuffs();

		// Add a debuff effect to the player
		void AddAppliedDebuff(IBuffSkill debuff);

		// Remove previously added debuff effect from the player
		void RemoveAppliedDebuff(IBuffSkill debuff);

		// Getter for player's experience
		int GetExperience();

		// Increase player's experience by given amount
		void IncreaseExperience(int amount);

		// Decrease player's experience by given amount
		void DecreaseExperience(int amount);

		// Getter for player's money
		int GetMoney();

		// Increase player's money by given amount
		void IncreaseMoney(int amount);

		// Getter for player's health potion count
		int GetHealthPotionCount();

		// Use health potion
		bool UseHealthPotion();

		// Getter for player's mana potion count
		int GetManaPotionCount();

		// Use mana potion
		bool UseManaPotion();

		// Getter for player's skills
		ISkill[] GetSkills();

		// Setter for player's skills
		void SetSkills(ISkill[] skills);

		// Getter for player's skill count
		int GetSkillCount();

		// Use the skill with given index
		bool UseSkill(int skillIndex, IPlayer target);

		// Getter for player's skill animation
		bool IsUsingSkill();

		// Setter for player's stun information
		void SetStunned(bool stunned);

		// Getter for player's stun information
		bool IsStunned();

		// Setter for player's dead information
		void SetDead(bool dead);

		// Getter for player's dead information
		bool IsDead();

		// Creates clone of player
		IPlayer ClonePlayer();

		// Resets player properties
		void ResetPlayer();

		// Getter for player's available information. !this.IsStunned() && !this.IsUsingSkill() && !this.IsAttacking() && !this.IsDead();
		bool IsAvailable();

		// Updates player's timers, skills and position
		void UpdatePlayer(float elapsedTime);

		// Used for collision detection in movement
		void SetCollisionManager(CollisionManager collisionManager);

	}

}