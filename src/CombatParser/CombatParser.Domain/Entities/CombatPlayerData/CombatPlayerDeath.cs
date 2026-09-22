namespace CombatParser.Domain.Entities.CombatPlayerData;

public record CombatPlayerDeath(
    TimeSpan Time,
    string Name,
    string Spell,
    long Value,
    long CurrentHealth,
    long MaxHealth,
    int Status,
    string UnitId
    );
