/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          SkillKind
 *   
 *   Description:    Each player character has various skills. In general, we
 *					 divided these skills into 3 groups. Skill kind represents
 *					 these 3 groups and determines which category a skill
 *					 belongs to.
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public enum SkillKind
	{
		ATTACK,
		BUFF,
		DEBUFF
	}

	public static class SkillKindExtensions 
	{
			public static int Count = 3;
	}
}