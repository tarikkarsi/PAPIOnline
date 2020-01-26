using MLAgents;

public class PlayerAcademy : Academy
{

	private BattleArena[] battleArenas;

	public override void AcademyReset()
	{
		// Get battle arenas
		if (battleArenas == null)
		{
			battleArenas = FindObjectsOfType<BattleArena>();
		}

		// Set-up arenas
		foreach (BattleArena battleArena in battleArenas)
		{
			battleArena.ResetArea();
		}
	}

}
