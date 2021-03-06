﻿/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          BuffKind
 *   
 *   Description:    Shows the properties of the players affected by buff skills
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

	public enum BuffKind
	{
		HEALTH,
		SPEED,
		MANA,
		DAMAGE,
		STUN,
		DEFENSE,
	}

	public static class BuffKindExtensions 
	{
		public static int Count = 6;
	}

}