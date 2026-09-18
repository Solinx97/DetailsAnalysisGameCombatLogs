namespace CombatParser.Domain.DTOs;

public record CombatPlayerPreAuraDto(
    string Id,
    string OwnerGameId,
    int GameId,
    string Name,
    int AbilityType,
    int Status,
    string UnitId
    );