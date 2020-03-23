public class PlayerMetrics
{
	private int buffCount;
	private int debuffCount;
	private float health;
	private float speed;
	private float damage;
	private float defense;
	private int stunned;

	public void Set(IPlayer player)
	{
		this.buffCount = player.GetBuffs().Count;
		this.debuffCount = player.GetDebuffs().Count;
		this.health = player.GetHealth();
		this.speed = player.GetSpeed();
		this.damage = player.GetDamage();
		this.defense = player.GetDefense();
		this.stunned = player.IsStunned() ? 1 : 0;
	}

	public int DiffBuffCount(IPlayer player)
	{
		return this.buffCount - player.GetBuffs().Count;
	}

	public int DiffDebuffCount(IPlayer player)
	{
		return this.debuffCount - player.GetDebuffs().Count;
	}

	public float DiffHealth(IPlayer player)
	{
		return this.health - player.GetHealth();
	}

	public int DiffStunned(IPlayer player)
	{
		return this.stunned - (player.IsStunned() ? 1 : 0);
	}

}
