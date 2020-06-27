/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          WarriorAgent
 *   
 *   Description:    Agent with warrior properties and skills
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

	public class WarriorAgent : PlayerAgent
	{

		public WarriorAgent(string name, bool requestDecision = true, bool manualReward = true)
			: base(name, WarriorProperties.GetWarriorProps(), WarriorProperties.GetWarriorSkills(), requestDecision, manualReward)
		{
		}

		public WarriorAgent() : this("Warrior")
		{
		}

	}

}