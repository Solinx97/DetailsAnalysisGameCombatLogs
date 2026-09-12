using CombatParser.Domain.Aggregates;
using CombatParser.Domain.EntityData;

namespace CombatParser.Domain.Entities;

public class CombatUnit : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int NAME_MAX_LENGTH = 128;

    private readonly List<UnitCast> _unitCasts = [];
    private readonly List<UnitPosition> _unitPositions = [];

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

    public IEnumerable<UnitCast> UnitCasts => _unitCasts;

    public IEnumerable<UnitPosition> UnitPositions => _unitPositions;

    public static CombatUnit Create(string gameId, string name, long health, string unitHash, int type, string? creatorGameId,
        IReadOnlyList<UnitCastData> unitCasts, IReadOnlyList<UnitPositionData> unitPositions)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(unitHash, nameof(unitHash));

        var combatUnit = new CombatUnit(gameId, name, health, unitHash, type, creatorGameId);

        foreach (var unitCast in unitCasts)
        {
            combatUnit.AddCast(unitCast);
        }

        foreach (var unitPosition in unitPositions)
        {
            combatUnit.AddUnitPosition(unitPosition);
        }

        return combatUnit;
    }

    private void AddCast(UnitCastData cast)
    {
        var createdCast = UnitCast.Create(cast.CreatorGameId, cast.GameSpellId, cast.Spell, cast.Time, cast.FinishTime,
            cast.TargetGameId, cast.IsImmediatly, cast.IsSuccess);
        _unitCasts.Add(createdCast);
    }

    private void AddUnitPosition(UnitPositionData unitPosition)
    {
        var createdPosition = UnitPosition.Create(unitPosition.CreatorGameId, unitPosition.X, unitPosition.Y,
            unitPosition.Time);
        _unitPositions.Add(createdPosition);
    }
}
