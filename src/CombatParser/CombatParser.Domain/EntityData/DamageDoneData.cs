namespace CombatParser.Domain.EntityData;

public record DamageDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string CreatorGameId,
    string TargetGameId,
    string TargetHash,
    long TargetCurrentHealth,
    int ModificationType,
    int DamageType,
    int Resisted,
    int Absorbed,
    int Blocked,
    int RealDamage,
    int Mitigated,
    int CombatPlayerId
    );
