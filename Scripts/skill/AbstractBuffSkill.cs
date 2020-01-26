public abstract class AbstractBuffSkill : AbstractSkill, IBuffSkill
{
	private static int BUFF_APPLY_TIMER = 1;

	// indicates buff skill’s duration
	protected float duration;

	// indicates buff skill’s amount
	protected int amount;

	// indicates buff skill’s buff kind
	protected BuffKind buffKind;

	// indicates debuff skill’s periodic property
	protected bool periodic;

	protected float buffApplyTimer;

	public AbstractBuffSkill(SkillKind skillKind, string name, int manaConsumption, float timeout, BuffKind buffKind, float duration, int amount, bool periodic)
		: base(skillKind, name, manaConsumption, timeout)
	{
		this.duration = duration;
		this.amount = amount;
		this.buffKind = buffKind;
		this.periodic = periodic;
		this.buffApplyTimer = BUFF_APPLY_TIMER;
	}

	protected AbstractBuffSkill()
	{
		// Empty constructor is only for cloning purposes
	}

	public int GetAmount()
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

	public override bool UseImpl(IPlayer source, IPlayer target)
	{
		if (target == null)
		{
			UnityEngine.Debug.Log("Skill " + this.name + " no target selected");
			return false;
		}

		if (this.IsBuff())
		{
			target.AddBuff(this.CloneBuffSkill());
		}
		else
		{
			target.AddDebuff(this.CloneBuffSkill());
		}
		
		return true;
	}

	public void UpdateBuff(IPlayer target, float elapsedTime)
	{
		// update timers
		this.buffApplyTimer -= elapsedTime;
		this.duration -= elapsedTime;

		if (this.duration > 0)
		{
			// one second passed 
			if (this.buffApplyTimer <= 0)
			{
				this.ApplyBuff(target);
				// reset timer
				this.buffApplyTimer = BUFF_APPLY_TIMER;
			}
		}
		else
		{
			this.ClearBuff(target);
			target.RemoveBuff(this);
		}
	}

	public void ApplyBuff(IPlayer target)
	{
		if (this.IsPeriodic())
		{
			this.ApplyBuffImpl(target, this.IsBuff());
		}
	}

	public void ClearBuff(IPlayer target)
	{
		if (!this.IsPeriodic())
		{
			// clear affect of debuff
			this.ApplyBuffImpl(target, !this.IsBuff());
		}
	}

	private void ApplyBuffImpl(IPlayer target, bool positive)
	{
		switch (this.GetBuffKind())
		{
			case BuffKind.HEALTH:
				{
					if (positive)
					{
						target.IncreaseHealth(this.amount);
					}
					else
					{
						target.DecreaseHealth(this.amount);
					}
					break;
				}
			case BuffKind.DAMAGE:
				{
					if (positive)
					{
						target.IncreaseDamage(this.amount);
					}
					else
					{
						target.DecreaseDamage(this.amount);
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
							playerTarget.IncreaseMana(this.amount);
						}
						else
						{
							playerTarget.DecreaseMana(this.amount);
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
			default:
				break;
		}
	}

	private bool IsBuff()
	{
		return this.skillKind == SkillKind.BUFF;
	}

	public override ISkill CloneSkill()
	{
		return CloneBuffSkill();
	}

	public void CloneAbstractBuffSkill(AbstractBuffSkill abstractBuffSkill)
	{
		// clone base
		CloneAbstractSkill(abstractBuffSkill);
		// clone own fields
		abstractBuffSkill.duration = this.duration;
		abstractBuffSkill.amount = this.amount;
		abstractBuffSkill.buffKind = this.buffKind;
		abstractBuffSkill.periodic = this.periodic;
		abstractBuffSkill.buffApplyTimer = this.buffApplyTimer;
	}

	public abstract IBuffSkill CloneBuffSkill();
}
