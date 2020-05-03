/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          AbstractSkill
 *   
 *   Description:    Abstract class for all skill implementations
 *   
 *   Author:         Tarik Karsi
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;

namespace PAPIOnline
{

	public abstract class AbstractSkill : ISkill
	{
		// Indicates skill’s kind
		protected SkillKind skillKind;

		// Indicates skill’s name
		protected string name;

		// Indicates skill’s mana consumption
		protected int manaConsumption;

		// Indicates skill’s timout in seconds
		protected float timeout;

		// Indicates skill’s availability
		protected bool available;

		// Timer that will be updated each update call
		private float timer;

		public AbstractSkill(SkillKind skillKind, string name, int manaConsumption, float timeout)
		{
			this.skillKind = skillKind;
			this.name = name;
			this.manaConsumption = manaConsumption;
			this.timeout = timeout;
			this.available = true;
			this.timer = 0;
		}

		protected AbstractSkill()
		{
			// Empty constructor is only for cloning purposes
		}

		public void ResetSkill()
		{
			this.available = true;
			this.timer = 0;
		}

		public SkillKind GetSkillKind()
		{
			return this.skillKind;
		}

		public string GetName()
		{
			return this.name;
		}

		public int GetManaConsumption()
		{
			return this.manaConsumption;
		}

		public float GetTimeout()
		{
			return this.timeout;
		}

		public bool Use(IPlayer source, IPlayer target)
		{
			bool result = false;
			if (this.IsAvailable() && source.GetMana() >= GetManaConsumption())
			{
				result = this.UseImpl(source, target);
				if (result)
				{
					// UnityEngine.Debug.Log("Skill used " + this.name);
					source.DecreaseMana(this.manaConsumption);
					this.available = false;
					this.timer = this.timeout;
				}
			}
			else
			{
				if (!this.IsAvailable())
				{
					Debug.LogError("Skill " + this.name + " is not available (AbstractSkill::Use)");
				}
				else
				{
					Debug.LogError(GetName() + " has not enough mana to use " + this.name + " (AbstractSkill::Use)");
				}
			}
			return result;
		}

		public bool IsAvailable()
		{
			return this.available;
		}

		public void Update(float elapsedTime)
		{
			if (!this.available)
			{
				timer -= elapsedTime;
				if (timer <= 0)
				{
					this.ResetSkill();
				}
			}
		}

		public void CloneAbstractSkill(AbstractSkill abstractSkill)
		{
			// Clone own fields
			abstractSkill.skillKind = this.skillKind;
			abstractSkill.name = this.name;
			abstractSkill.manaConsumption = this.manaConsumption;
			abstractSkill.timeout = this.timeout;
			abstractSkill.available = this.available;
			abstractSkill.timer = this.timer;
		}

		public abstract ISkill CloneSkill();

		public abstract bool UseImpl(IPlayer source, IPlayer target);

	}

}