namespace CombatParser.Domain.EntityData;

public record UnitData(
    string GameId,
    string Name,
    string UnitHash,
    int Type,
    string? CreatorGameId,
    int CombatId,
    IReadOnlyList<UnitHealthData> UnitHealths,
    IReadOnlyList<UnitCastData> UnitCasts,
    IReadOnlyList<UnitPositionData> UnitPositions
    );