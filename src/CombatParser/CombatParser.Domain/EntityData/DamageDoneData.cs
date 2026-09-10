namespace CombatParser.Domain.EntityData;

public record DamageDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string CreatorId,
    string CreatorGameId,
    string TargetId,
    string TargetGameId,
    int ModificationType,
    int DamageType,
    int Resisted,
    int Absorbed,
    int Blocked,
    int RealDamage,
    int Overkill,
    int Mitigated,
    int CombatPlayerId
    );
