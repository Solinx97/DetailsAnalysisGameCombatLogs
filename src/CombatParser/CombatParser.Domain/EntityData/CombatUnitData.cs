namespace CombatParser.Domain.EntityData;

public record CombatUnitData(
    string GameId,
    string Username,
    string? CreatorGameId,
    int CombatId
    );