namespace CombatParser.Domain.EntityData;

public record DamageDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string Creator,
    string Target,
    bool IsTargetBoss,
    int DamageType,
    bool IsPeriodicDamage,
    bool IsSingleTarget,
    bool IsPet,
    int CombatPlayerId
    );
