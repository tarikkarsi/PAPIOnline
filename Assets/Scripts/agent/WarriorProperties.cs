/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorProperties
 *   
 *   Description:    Warrior player properties
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public class WarriorProperties
	{

		public static PlayerProperties GetWarriorProps()
		{ 
			return new PlayerProperties.PlayerPropertiesBuilder()
			.HealthCapacity(500)
			.Health(500)
			.ManaCapacity(500)
			.AttackRange(3)
			.Mana(500)
			.Defense(10)
			.Damage(14)
			.Speed(10)
			.HealthPotionCount(10)
			.ManaPotionCount(10)
			.Build();
		}

		public static ISkill[] GetWarriorSkills()
		{
			return new ISkill[]
			{
			// attack skill with low damage
			new AttackSkill(
				"Attack1",  // name
				10,         // mana consumption
				3,          // timeout
				15,         // damage
				7           // range
			),

			// attack skill with stun debuff effect
			new AttackSkill(
				"Attack2",  // name
				15,         // mana consumption
				4,          // timeout
				20,         // damage
				7,          // range
				new DebuffSkill(    // debuff
					"Debuff1",      // name
					0,              // mana consumption
					0,              // timeout
					BuffKind.STUN,  // buff kind
					2,              // duration
					0,              // amount
					false           // periodic
				),
				100         // debuff percentage
			),

			// attack skill with periodic health debuff effect
			new AttackSkill(
				"Attack3",  // name
				20,         // mana consumption
				5,          // timeout
				25,         // damage
				7,          // range
				new DebuffSkill(    // debuff
					"Debuff2",      // name
					0,              // mana consumption
					0,              // timeout
					BuffKind.HEALTH,// buff kind
					10,             // duration
					4,              // amount
					true            // periodic
				),
				10          // debuff percentage
			),

			// attack skill with moderate heavy damage
			new AttackSkill(
				"Attack4",  // name
				25,         // mana consumption
				6,          // timeout
				35,         // damage
				7           // range
			),

			// attack skill with heavy damage
			new AttackSkill(
				"Attack5",	// name
				35,         // mana consumption
				7,          // timeout
				45,         // damage
				7           // range
			),

			// buff skill which enhances damage
			new BuffSkill(
				"Buff1",		// name
				60,				// mana consumption
				60,				// timeout
				BuffKind.DAMAGE,// buff kind
				60,				// duration
				2				// amount
			),

			// buff skill which enhances speed
			new BuffSkill(
				"Buff2",        // name
				70,             // mana consumption
				60,             // timeout
				BuffKind.SPEED, // buff kind
				60,             // duration
				5f              // amount
			)
		};
		}

	}

}