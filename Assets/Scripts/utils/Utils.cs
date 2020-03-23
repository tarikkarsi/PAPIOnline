using UnityEngine;
using System.Collections.Generic;

public static class Utils
{

	public static float GetDistance(Vector3 from, Vector3 to)
	{
		return Vector3.Distance(from, to);
	}

	public static Vector3 GetDirection(Vector3 from, Vector3 to)
	{
		return (to - from).normalized;
	}

	public static bool PositionEquals(Vector3 pos1, Vector3 pos2)
	{
		// only control x and z positons
		int pos1x = (int)pos1.x;
		int pos2x = (int)pos2.x;
		int pos1z = (int)pos1.z;
		int pos2z = (int)pos2.z;
		bool result = pos1x == pos2x && pos1z == pos2z;
		Debug.Log("result: " + result);
		return result;
	}

	/*
	 * This method returns array of indices of not available movements on the next iteration
	 * Checks character stunned or not, if stunned no movement will be available
	 */
	public static int[] GetMoveMasks(IPlayer player)
	{
		// player can not move when stunned
		// starts from 1 because of zero is idle
		return player.IsAvailable() ? new int[] { 1, 2, 3, 4 } : new int[0];
	}

	/*
	 * This method returns array of indices of not available skills on the next iteration
	 * Checks characher stunned or not, if stunned no skill can be used
	 * Checks distance for attack skills, if not close enough, skill disabled
	 */
	public static int[] GetSkillMasks(IPlayer player, IPlayer enemy)
	{
		// player can not use any skills and attack when stunned, dead or animating
		if (player.IsAvailable())
		{
			return new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
		}

		List<int> skillMasks = new List<int>();
		// add skill masks
		int i = 0;
		ISkill[] skills = player.GetSkills();
		for (; i < skills.Length; i++)
		{
			ISkill skill = skills[i];
			// player can not use any skills when stunned
			// check player close enough to use attack skill
			if (!skill.IsAvailable() || (skill.GetSkillKind() == SkillKind.ATTACK &&
				Utils.GetDistance(player.GetPosition(), enemy.GetPosition()) > ((IAttackSkill)skill).GetRange()))
			{
				// starts from 1 because of zero is idle
				skillMasks.Add(i + 1);
			}
		}

		// add attack mask after skill indexes
		if (Utils.GetDistance(player.GetPosition(), enemy.GetPosition()) > player.GetAttackRange())
		{
			skillMasks.Add(i + 1);
		}

		return skillMasks.ToArray();
	}

}
