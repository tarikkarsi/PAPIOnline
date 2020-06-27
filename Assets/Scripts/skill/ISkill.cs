/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          ISkill
 *   
 *   Description:    All the skills in our game must implement the skill
 *					 interface which includes common features found in all of
 *					 the skills. Each skill has a timeout value and sets the
 *					 minimum duration between consecutive uses. Skills need mana
 *					 to be used. They cannot be used if there is not enough mana.
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

	public interface ISkill
	{
		// Getter for skill's kind
		SkillKind GetSkillKind();

		// Getter for skill's name
		string GetName();

		// Getter for skill's mana consumption
		int GetManaConsumption();

		// Getter for skill's timeout in seconds
		float GetTimeout();

		// Getter for skill's availability
		bool IsAvailable();

		// Use this skill from source to destination
		bool Use(IPlayer source, IPlayer target);

		// Update buff
		void Update(float elapsedTime);

		// Reset skill
		void ResetSkill();

		// Creates clone of skill
		ISkill CloneSkill();

	}

}