/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          IBuffSkill
 *   
 *   Description:    Features that distinguish buff/debuff skills from other
 *					 skills are found in the buff skill interface. All
 *					 buff/debuff skills in our game must implement this
 *					 interface. Buff skills and debuff skills work with the same
 *					 logic, although they have different effects on players.
 *					 Buff has a positive effect, while debuff has a negative
 *					 effect. Buff/Debuff skills have a certain amount of impact
 *					 during duration. This effect can be permanent, temporary or
 *					 periodic. While the effect of temporary ones comes back
 *					 when they are deleted, but in permanent ones, this effect
 *					 does not come back. Finally, periodic ones have more than
 *					 one effect during duration.
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
namespace PAPIOnline
{

	public interface IBuffSkill : ISkill
	{
		// Getter for buff skill's duration in seconds
		float GetDuration();

		// Getter for buff skill's amount
		float GetAmount();

		// Getter for buff skill's buff kind
		BuffKind GetBuffKind();

		// Getter for debuff skill's periodic property
		bool IsPeriodic();

		// Adds buff to target
		void AddBuff(IPlayer target);

		// Update buff for given target
		void UpdateBuff(IPlayer target, float elapsedTime);

		// Applies the efect of the buff
		void ApplyBuff(IPlayer target);

		// Clear the effect of the buff
		void ClearBuff(IPlayer target);

		// Remove the buff from target
		void RemoveBuff(IPlayer target);

		// Each buff will be added to character as clone
		IBuffSkill CloneBuffSkill();

	}

}