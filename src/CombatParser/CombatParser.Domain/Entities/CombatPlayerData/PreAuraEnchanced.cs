namespace CombatParser.Domain.Entities.CombatPlayerData;

public record PreAuraEnchanced(
    string Id,
    string OwnerGameId,
    int GameId,
    string Name,
    int AbilityType,
    int Status,
    string UnitId
    );