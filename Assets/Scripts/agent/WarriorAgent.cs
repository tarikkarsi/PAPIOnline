/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorAgent
 *   
 *   Description:    Agent with warrior properties and skills
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/

namespace PAPIOnline
{

	public class WarriorAgent : PlayerAgent
	{

		public WarriorAgent(string name, bool requestDecision = true)
			: base(name, WarriorProperties.warriorProps, WarriorProperties.warriorSkills, requestDecision)
		{
		}

		public WarriorAgent() : this("Warrior")
		{
		}

	}

}