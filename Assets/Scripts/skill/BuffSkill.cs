/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          BuffSkill
 *   
 *   Description:    Buff skill implementation
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
namespace PAPIOnline
{

	public class BuffSkill : AbstractBuffSkill
	{

		public BuffSkill(string name, int manaConsumption, float timeout, BuffKind buffKind, float duration, float amount)
			: base(SkillKind.BUFF, name, manaConsumption, timeout, buffKind, duration, amount, false)
		{
		}

		private BuffSkill()
		{
			// Empty constructor is only for cloning purposes
		}

		public override bool UseImpl(IPlayer source, IPlayer target)
		{
			// Add buff skills to the source
			this.AddBuff(source);
			this.ApplyBuff(source);
			return true;
		}

		public override void AddBuff(IPlayer target)
		{
			target.AddAppliedBuff(this.CloneBuffSkill());
		}

		public override void ApplyBuff(IPlayer target)
		{
			this.ApplyBuff(target, true);
		}

		public override void ClearBuff(IPlayer target)
		{
			this.ApplyBuff(target, false);
		}

		public override void RemoveBuff(IPlayer target)
		{
			target.RemoveAppliedBuff(this);
		}

		public override IBuffSkill CloneBuffSkill()
		{
			BuffSkill clone = new BuffSkill();
			CloneAbstractBuffSkill(clone);
			return clone;
		}

	}

}