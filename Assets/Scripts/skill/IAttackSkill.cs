
namespace PAPIOnline
{

	public interface IAttackSkill : ISkill
	{
		// getter for attack skill's power
		int GetDamage();

		// getter for attack skill's range
		int GetRange();

		// getter for attack skill's debuff effect
		IBuffSkill GetDebuff();

		// getter for attack skill's debuff effect possibility
		int GetDebuffPercentage();

		// getter for attack skills debuff effect existence
		bool HasDebuff();

	}

}