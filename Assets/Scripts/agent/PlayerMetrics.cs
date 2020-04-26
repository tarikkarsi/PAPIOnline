
namespace PAPIOnline
{

	public class PlayerMetrics
	{
		private bool[] skillAvailabilities;
		private int debuffCount;
		private float health;
		private int healthPotionCount;
		private int manaPotionCount;
		private bool isAttacking;

		public void Set(IPlayer player)
		{
			skillAvailabilities = new bool[player.GetSkillCount()];
			for (int i = 0; i < player.GetSkillCount(); i++)
			{
				skillAvailabilities[i] = player.GetSkills()[i].IsAvailable();
			}

			this.debuffCount = player.GetAppliedDebuffs().Count;
			this.health = player.GetHealth();
			this.healthPotionCount = player.GetHealthPotionCount();
			this.manaPotionCount = player.GetManaPotionCount();
			this.isAttacking = player.IsAttacking();
		}

		public int DiffDebuffCount(IPlayer player)
		{
			return this.debuffCount - player.GetAppliedDebuffs().Count;
		}

		public float DiffHealth(IPlayer player)
		{
			return this.health - player.GetHealth();
		}

		public int DiffHealthPotionCount(IPlayer player)
		{
			return this.healthPotionCount - player.GetHealthPotionCount();
		}

		public int DiffManaPotionCount(IPlayer player)
		{
			return this.manaPotionCount - player.GetManaPotionCount();
		}

		public bool IsAttacked(IPlayer player)
		{
			// check switch to attacking
			return !isAttacking && player.IsAttacking();
		}

		public int GetUsedSkillIndex(IPlayer player)
		{
			int usedSkillIndex = -1;

			// previous available, current not available means skill is used
			for (int i = 0; i < player.GetSkillCount(); i++)
			{
				if (skillAvailabilities[i] && !player.GetSkills()[i].IsAvailable())
				{
					usedSkillIndex = i;
					break;
				}
			}

			return usedSkillIndex;
		}

	}

}