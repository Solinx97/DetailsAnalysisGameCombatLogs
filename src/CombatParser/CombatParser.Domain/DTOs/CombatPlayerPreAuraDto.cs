namespace CombatParser.Domain.DTOs;

public record CombatPlayerPreAuraDto(
    string CreatorGameId,
    int GameId,
    string Name,
    int AbilityType,
    int Status,
    int CombatPlayerId
    );