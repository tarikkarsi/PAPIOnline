public interface ISkill
{
	// getter for skill’s kind
	SkillKind GetSkillKind();

	// getter for skill’s name
	string GetName();

	// getter for skill’s mana consumption
	int GetManaConsumption();

	// getter for skill’s timeout in seconds
	float GetTimeout();

	// getter for skill’s availability
	bool IsAvailable();

	// use this skill from source to destination
	bool Use(IPlayer source, IPlayer target);

	// update buff
	void Update(float elapsedTime);

	// reset skill
	void ResetSkill();

	// creates clone of skill
	ISkill CloneSkill();

}
