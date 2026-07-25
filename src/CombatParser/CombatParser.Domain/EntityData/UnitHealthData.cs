namespace CombatParser.Domain.EntityData;

public record UnitHealthData(
    string GamePlayerId,
    int CurrentHealth,
    int MaxHealth,
    TimeSpan Time,
    bool IsDead,
    int CombatId
    );
