using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class CombatUnit : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int NAME_MAX_LENGTH = 128;

    private CombatUnit() { }

    private CombatUnit(string gameId, string name, long health, string unitHash, int type, string? creatorGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Name = name;
        Health = health;
        UnitHash = unitHash;
        Type = type;
        CreatorGameId = creatorGameId;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public long Health { get; private set; }

    public string UnitHash { get; private set; }

    public int Type { get; private set; }

    public string? CreatorGameId { get; private set; }

    public Combat Combat { get; private set; }

    public static CombatUnit Create(string gameId, string name, long health, string unitHash, int type, string? creatorGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(unitHash, nameof(unitHash));

        return new CombatUnit(gameId, name, health, unitHash, type, creatorGameId);
    }
}
