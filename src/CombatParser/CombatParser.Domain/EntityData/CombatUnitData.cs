namespace CombatParser.Domain.EntityData;

public record CombatUnitData(
    string GameId,
    string Name,
    long Health,
    string UnitHash,
    int Type,
    string? CreatorGameId,
    int CombatId,
    IReadOnlyList<UnitCastData> UnitCasts,
    IReadOnlyList<UnitPositionData> UnitPositions
    );