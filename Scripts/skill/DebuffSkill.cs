public class DebuffSkill : AbstractBuffSkill
{

	public DebuffSkill(string name, int manaConsumption, float timeout, BuffKind buffKind, float duration, int amount, bool periodic)
		: base(SkillKind.DEBUFF, name, manaConsumption, timeout, buffKind, duration, amount, periodic)
	{
		this.periodic = periodic;
	}

	private DebuffSkill()
	{

	}

	public override IBuffSkill CloneBuffSkill()
	{
		DebuffSkill clone = new DebuffSkill();
		CloneAbstractBuffSkill(clone);
		return clone;
	}

}
