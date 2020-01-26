public class BuffSkill : AbstractBuffSkill
{

	public BuffSkill(string name, int manaConsumption, float timeout, BuffKind buffKind, float duration, int amount)
		: base(SkillKind.BUFF, name, manaConsumption, timeout, buffKind, duration, amount, false)
	{
	}

	private BuffSkill()
	{
		// Empty constructor is only for cloning purposes
	}

	public override IBuffSkill CloneBuffSkill()
	{
		BuffSkill clone = new BuffSkill();
		CloneAbstractBuffSkill(clone);
		return clone;
	}

}
