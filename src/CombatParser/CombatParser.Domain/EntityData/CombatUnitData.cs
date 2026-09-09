namespace CombatParser.Domain.EntityData;

public record CombatUnitData(
    string GameId,
    string Name,
    long Health,
    string UnitHash,
    string? CreatorGameId,
    int CombatId
    );