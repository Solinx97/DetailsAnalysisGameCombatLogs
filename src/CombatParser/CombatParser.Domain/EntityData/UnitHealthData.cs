namespace CombatParser.Domain.EntityData;

public record UnitHealthData(
    string GameId,
    int CurrentHealth,
    int MaxHealth,
    TimeSpan Time,
    bool IsDead,
    int CombatId
    );
