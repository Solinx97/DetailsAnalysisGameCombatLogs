namespace CombatParser.Domain.EntityData;

public record UnitHealthData(
    string OwnerGameId,
    long CurrentHealth,
    long MaxHealth,
    int Status,
    TimeSpan Time
    );
