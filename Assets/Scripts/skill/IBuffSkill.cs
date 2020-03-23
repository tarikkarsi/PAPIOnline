public interface IBuffSkill : ISkill
{
	// getter for buff skill’s duration in seconds
	float GetDuration();

	// getter for buff skill’s amount
	int GetAmount();

	// getter for buff skill’s buff kind
	BuffKind GetBuffKind();

	// getter for debuff skill’s periodic property
	bool IsPeriodic();

	// update buff for given target
	void UpdateBuff(IPlayer target, float elapsedTime);

	// called each second until duration runs out
	void ApplyBuff(IPlayer target);

	// called at the end of duration
	void ClearBuff(IPlayer target);

	// eact buff will be added to character as clone
	IBuffSkill CloneBuffSkill();

}
