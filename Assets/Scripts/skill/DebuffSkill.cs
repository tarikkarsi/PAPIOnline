
namespace PAPIOnline
{

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

		public override bool UseImpl(IPlayer source, IPlayer target)
		{
			// add debuff skills to the target
			this.AddBuff(target);
			this.ApplyBuff(target);
			return true;
		}

		public override void AddBuff(IPlayer target)
		{
			target.AddAppliedDebuff(this.CloneBuffSkill());
		}

		public override void ApplyBuff(IPlayer target)
		{
			this.ApplyBuff(target, false);
		}

		public override void ClearBuff(IPlayer target)
		{
			this.ApplyBuff(target, true);
		}

		public override void RemoveBuff(IPlayer target)
		{
			target.RemoveAppliedDebuff(this);
		}

		public override IBuffSkill CloneBuffSkill()
		{
			DebuffSkill clone = new DebuffSkill();
			CloneAbstractBuffSkill(clone);
			return clone;
		}

	}

}