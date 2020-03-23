using System;

public class AttackSkill : AbstractSkill, IAttackSkill
{
	// indicates attack skill’s power
	protected int power;

	// indicates attack skill’s range
	protected int range;

	// indicates attack skill’s debuff effect
	private IBuffSkill debuff;

	// indicates attack skill’s debuff effect possibility
	private int debuffPercentage;

	public AttackSkill(string name, int manaConsumption, float timeout, int power, int range)
		: this(name, manaConsumption, timeout, power, range, null, 0)
	{
	}

	public AttackSkill(string name, int manaConsumption, float timeout, int power, int range, IBuffSkill debuff, int debuffPercentage)
		: base(SkillKind.ATTACK, name, manaConsumption, timeout)
	{
		this.power = power;
		this.range = range;
		this.debuff = debuff;
		this.debuffPercentage = debuffPercentage;
	}

	private AttackSkill()
	{
		// Empty constructor is only for cloning purposes
	}

	public int GetPower()
	{
		return this.power;
	}

	public int GetRange()
	{
		return this.range;
	}

	public IBuffSkill GetDebuff()
	{
		return this.debuff;
	}

	public int GetDebuffPercentage()
	{
		return this.debuffPercentage;
	}

	public bool HasDebuff()
	{
		return this.debuff != null;
	}

	public override bool UseImpl(IPlayer source, IPlayer target)
	{
		if (target != null)
		{
			float distance = Utils.GetDistance(source.GetPosition(), target.GetPosition());
			if (distance < this.range)
			{
				// TODO make damage calculation
				target.DecreaseHealth(this.power);

				// Use debuff
				if (this.HasDebuff() && this.UseDebuff())
				{
					UnityEngine.Debug.Log("Skill " + this.name + " debuff applied");
					this.debuff.Use(source, target);
				}
				return true;
			}
			else
			{
				UnityEngine.Debug.Log("Skill " + this.name + " is out of range");
			}
		}
		else
		{
			UnityEngine.Debug.Log("Skill " + this.name + " no enemy selected");
		}

		return false;
	}

	private bool UseDebuff()
	{
		Random random = new Random();
		int value = random.Next(0, 100);
		return value < this.debuffPercentage;
	}

	public override ISkill CloneSkill()
	{
		AttackSkill clone = new AttackSkill();
		// clone base fields
		CloneAbstractSkill(clone);
		// clone own fields
		clone.power = this.power;
		clone.range = this.range;
		clone.debuff = this.debuff;
		clone.debuffPercentage = this.debuffPercentage;
		return clone;
	}
}
