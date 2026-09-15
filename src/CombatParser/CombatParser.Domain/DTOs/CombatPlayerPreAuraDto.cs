namespace CombatParser.Domain.DTOs;

public record CombatPlayerPreAuraDto(
    string Id,
    string CreatorGameId,
    int GameId,
    string Name,
    int AbilityType,
    int Status,
    string UnitId
    );