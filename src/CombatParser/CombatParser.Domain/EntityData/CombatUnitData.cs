namespace CombatParser.Domain.EntityData;

public record CombatUnitData(
    string GameId,
    string Username,
    string? CreatorGameId,
    string? UnitType,
    int CombatId
    );