namespace CombatParser.Domain.DTOs;

public record CombatPlayerPreAuraDto(
    int Id,
    string CreatorGameId,
    int GameId,
    string Name,
    int AbilityType,
    int Status,
    int CombatPlayerId
    );