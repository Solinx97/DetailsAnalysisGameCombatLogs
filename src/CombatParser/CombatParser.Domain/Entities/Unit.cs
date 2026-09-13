using CombatParser.Domain.Aggregates;
using CombatParser.Domain.EntityData;

namespace CombatParser.Domain.Entities;

public class Unit : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int NAME_MAX_LENGTH = 128;

    private readonly List<UnitHealth> _unitHealthes = [];
    private readonly List<UnitCast> _unitCasts = [];
    private readonly List<UnitPosition> _unitPositions = [];

    private Unit() { }

    private Unit(string gameId, string name, string unitHash, int type, string? creatorGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Name = name;
        UnitHash = unitHash;
        Type = type;
        CreatorGameId = creatorGameId;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string UnitHash { get; private set; }

    public int Type { get; private set; }

    public string? CreatorGameId { get; private set; }

    public Combat Combat { get; private set; }

    public IEnumerable<UnitHealth> UnitHealthes => _unitHealthes;

    public IEnumerable<UnitCast> UnitCasts => _unitCasts;

    public IEnumerable<UnitPosition> UnitPositions => _unitPositions;

    public static Unit Create(string gameId, string name, string unitHash, int type, string? creatorGameId,
        IReadOnlyList<UnitHealthData> unitHealthes, IReadOnlyList<UnitCastData> unitCasts, IReadOnlyList<UnitPositionData> unitPositions)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(unitHash, nameof(unitHash));

        var combatUnit = new Unit(gameId, name, unitHash, type, creatorGameId);

        foreach (var unitHealth in unitHealthes)
        {
            combatUnit.AddHealth(unitHealth);
        }


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

    private void AddHealth(UnitHealthData health)
    {
        var createdHealth = UnitHealth.Create(health.OwnerGameId, health.CurrentHealth, health.MaxHealth, health.Time);
        _unitHealthes.Add(createdHealth);
    }

    private void AddCast(UnitCastData cast)
    {
        var createdCast = UnitCast.Create(cast.OwnerGameId, cast.GameSpellId, cast.Spell, cast.Time, cast.FinishTime,
            cast.TargetGameId, cast.IsImmediatly, cast.IsSuccess);
        _unitCasts.Add(createdCast);
    }

    private void AddUnitPosition(UnitPositionData unitPosition)
    {
        var createdPosition = UnitPosition.Create(unitPosition.OwnerGameId, unitPosition.X, unitPosition.Y,
            unitPosition.Time);
        _unitPositions.Add(createdPosition);
    }
}
