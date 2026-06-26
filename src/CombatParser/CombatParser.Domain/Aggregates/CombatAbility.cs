namespace CombatParser.Domain.Aggregates;

public class CombatAbility
{
    private CombatAbility() { }

    private CombatAbility(int id, int gameId, string name, int abilityType)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        AbilityType = abilityType;
    }

    public int Id { get; private set; }

    public int GameId { get; private set; }

    public string Name { get; private set; }

    public int AbilityType { get; private set; }

    public static CombatAbility Create(int id, int gameId, string name, int abilityType)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        return new CombatAbility(id, gameId, name, abilityType);
    }
}
