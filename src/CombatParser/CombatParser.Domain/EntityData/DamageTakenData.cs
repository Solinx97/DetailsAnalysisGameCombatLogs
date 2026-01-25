namespace CombatParser.Domain.EntityData;

public record DamageTakenData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string Creator,
    string Target,
    int DamageTakenType,
    int ActualValue,
    bool IsPeriodicDamage,
    int Resisted,
    int Absorbed,
    int Blocked,
    int RealDamage,
    int Mitigated,
    int CombatPlayerId
    );
