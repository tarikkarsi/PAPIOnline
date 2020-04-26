using System;
using UnityEngine;

namespace PAPIOnline
{

	public class AttackSkill : AbstractSkill, IAttackSkill
	{
		// indicates attack skill’s power
		protected int damage;

		// indicates attack skill’s range
		protected int range;

		// indicates attack skill’s debuff effect
		private DebuffSkill debuff;

		// indicates attack skill’s debuff effect possibility
		private int debuffPercentage;

		public AttackSkill(string name, int manaConsumption, float timeout, int power, int range)
			: this(name, manaConsumption, timeout, power, range, null, 0)
		{
		}

		public AttackSkill(string name, int manaConsumption, float timeout, int damage, int range, DebuffSkill debuff, int debuffPercentage)
			: base(SkillKind.ATTACK, name, manaConsumption, timeout)
		{
			this.damage = damage;
			this.range = range;
			this.debuff = debuff;
			this.debuffPercentage = debuffPercentage;
		}

		private AttackSkill()
		{
			// Empty constructor is only for cloning purposes
		}

		public int GetDamage()
		{
			return this.damage;
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
			float distance = Utils.GetDistance(source, target);
			if (distance < this.range)
			{
				// TODO make damage calculation
				target.DecreaseHealth(this.damage);

				// Use debuff
				if (this.HasDebuff() && this.UseDebuff())
				{
					// UnityEngine.Debug.Log("Skill " + this.name + " debuff applied");
					this.debuff.UseImpl(source, target);
				}
				return true;
			}
			else
			{
				Debug.LogError("Skill " + this.name + " is out of range");
			}

			return false;
		}

		private bool UseDebuff()
		{
			System.Random random = new System.Random();
			int value = random.Next(0, 100);
			return value < this.debuffPercentage;
		}

		public override ISkill CloneSkill()
		{
			AttackSkill clone = new AttackSkill();
			// clone base fields
			CloneAbstractSkill(clone);
			// clone own fields
			clone.damage = this.damage;
			clone.range = this.range;
			clone.debuff = this.debuff;
			clone.debuffPercentage = this.debuffPercentage;
			return clone;
		}
	}

}