/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          AbstractBuffSkill
 *   
 *   Description:    Abstract class for buff and debuff skill implementations
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

	public abstract class AbstractBuffSkill : AbstractSkill, IBuffSkill
	{
		private static int PERIODIC_APPLY_TIMER = 1;

		// Indicates buff skill’s duration
		protected float duration;

		// Indicates buff skill’s amount
		protected float amount;

		// Indicates buff skill’s buff kind
		protected BuffKind buffKind;

		// Indicates debuff skill’s periodic property
		protected bool periodic;

		protected float periodicApplyTimer;

		public AbstractBuffSkill(SkillKind skillKind, string name, int manaConsumption, float timeout, BuffKind buffKind, float duration, float amount, bool periodic)
			: base(skillKind, name, manaConsumption, timeout)
		{
			this.duration = duration;
			this.amount = amount;
			this.buffKind = buffKind;
			this.periodic = periodic;
			this.periodicApplyTimer = PERIODIC_APPLY_TIMER;
		}

		protected AbstractBuffSkill()
		{
			// Empty constructor is only for cloning purposes
		}

		public float GetAmount()
		{
			return this.amount;
		}

		public BuffKind GetBuffKind()
		{
			return this.buffKind;
		}

		public float GetDuration()
		{
			return this.duration;
		}

		public bool IsPeriodic()
		{
			return this.periodic;
		}

		public void UpdateBuff(IPlayer target, float elapsedTime)
		{
			// Update timers
			this.periodicApplyTimer -= elapsedTime;
			this.duration -= elapsedTime;

			if (this.duration > 0)
			{
				// One second passed
				if (IsPeriodic() && this.periodicApplyTimer <= 0)
				{
					this.ApplyBuff(target);
					// Reset timer
					this.periodicApplyTimer = PERIODIC_APPLY_TIMER;
				}
			}
			else
			{
				// Remove effect of non periodic buffs
				if (!this.IsPeriodic())
				{
					this.ClearBuff(target);
				}
				this.RemoveBuff(target);
			}
		}

		protected void ApplyBuff(IPlayer target, bool positive)
		{
			switch (this.GetBuffKind())
			{
				case BuffKind.HEALTH:
					{
						if (positive)
						{
							target.IncreaseHealth((int)this.amount);
						}
						else
						{
							target.DecreaseHealth((int)this.amount);
						}
						break;
					}
				case BuffKind.DAMAGE:
					{
						if (positive)
						{
							target.IncreaseDamage((int)this.amount);
						}
						else
						{
							target.DecreaseDamage((int)this.amount);
						}
						break;
					}
				case BuffKind.SPEED:
					{
						if (positive)
						{
							target.IncreaseSpeed(this.amount);
						}
						else
						{
							target.DecreaseSpeed(this.amount);
						}
						break;
					}
				case BuffKind.MANA:
					{
						if (target is IPlayer)
						{
							IPlayer playerTarget = target as IPlayer;
							if (positive)
							{
								playerTarget.IncreaseMana((int)this.amount);
							}
							else
							{
								playerTarget.DecreaseMana((int)this.amount);
							}
						}
						break;
					}
				case BuffKind.STUN:
					{
						if (positive)
						{
							target.SetStunned(false);
						}
						else
						{
							target.SetStunned(true);
						}
						break;
					}
				case BuffKind.DEFENSE:
					{
						if (positive)
						{
							target.IncreaseDefense((int)this.amount);
						}
						else
						{
							target.DecreaseDefense((int)this.amount);
						}
						break;
					}
				default:
					break;
			}
		}

		public override ISkill CloneSkill()
		{
			return CloneBuffSkill();
		}

		public void CloneAbstractBuffSkill(AbstractBuffSkill abstractBuffSkill)
		{
			// Clone base
			CloneAbstractSkill(abstractBuffSkill);
			// Clone own fields
			abstractBuffSkill.duration = this.duration;
			abstractBuffSkill.amount = this.amount;
			abstractBuffSkill.buffKind = this.buffKind;
			abstractBuffSkill.periodic = this.periodic;
			abstractBuffSkill.periodicApplyTimer = this.periodicApplyTimer;
		}

		public abstract void AddBuff(IPlayer target);

		public abstract void ApplyBuff(IPlayer target);

		public abstract void ClearBuff(IPlayer target);

		public abstract void RemoveBuff(IPlayer target);

		public abstract IBuffSkill CloneBuffSkill();
	}

}