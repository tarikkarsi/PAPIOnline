using System.Collections.Generic;

public class Stats
{
	public int action;
	public int n_plays;
	public int n_wins;
	public List<Stats> children;

	public Stats(int action, int n_plays, int n_wins)
	{
		this.action = action;
		this.n_plays = n_plays;
		this.n_wins = n_wins;
		this.children = new List<Stats>();
	}
}
