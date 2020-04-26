
namespace PAPIOnline
{

	public interface IBuffSkill : ISkill
	{
		// getter for buff skill's duration in seconds
		float GetDuration();

		// getter for buff skill's amount
		float GetAmount();

		// getter for buff skill's buff kind
		BuffKind GetBuffKind();

		// getter for debuff skill's periodic property
		bool IsPeriodic();

		// adds buff to target
		void AddBuff(IPlayer target);

		// update buff for given target
		void UpdateBuff(IPlayer target, float elapsedTime);

		// applies the efect of the buff
		void ApplyBuff(IPlayer target);

		// clear the effect of the buff
		void ClearBuff(IPlayer target);

		// remove the buff from target
		void RemoveBuff(IPlayer target);

		// each buff will be added to character as clone
		IBuffSkill CloneBuffSkill();

	}

}