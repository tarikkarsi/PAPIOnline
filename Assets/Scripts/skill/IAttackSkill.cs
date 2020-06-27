/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          IAttackSkill
 *   
 *   Description:    Features that distinguish attack skills from other skills
 *					 are found in the attack skill interface. All attack skills
 *					 in our game must implement this interface. The aim of the
 *					 attack skills is to damage the opponent and they need a
 *					 certain closeness to use it. Attack skills can have a
 *					 debuff effect. This effect may occur with a certain
 *					 probability with the application of the attack skill.
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

	public interface IAttackSkill : ISkill
	{
		// Getter for attack skill's power
		int GetDamage();

		// Getter for attack skill's range
		int GetRange();

		// Getter for attack skill's debuff effect
		IBuffSkill GetDebuff();

		// Getter for attack skill's debuff effect possibility
		int GetDebuffPercentage();

		// Getter for attack skills debuff effect existence
		bool HasDebuff();

	}

}