namespace CombatParser.Domain.Aggregates;

public class CombatAbility
{
    private CombatAbility() { }

    public CombatAbility(int id, int gameId, string name, int abilityType)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        Id = id;
        GameId = gameId;
        Name = name;
        AbilityType = abilityType;
    }

    public int Id { get; private set; }

    public int GameId { get; private set; }

    public string Name { get; private set; }

    public int AbilityType { get; private set; }
}
