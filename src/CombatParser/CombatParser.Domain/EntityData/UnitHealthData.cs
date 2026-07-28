namespace CombatParser.Domain.EntityData;

public record UnitHealthData(
    string CreatorGameId,
    int CurrentHealth,
    int MaxHealth,
    TimeSpan Time,
    bool IsDead,
    int CombatId
    );
