namespace CombatParser.Domain.EntityData;

public record DamageDoneGeneralData(
    int GameSpellId,
    string Spell,
    int Value, 
    double DamagePerSecond,
    int CritNumber, 
    int MissNumber, 
    int CastNumber, 
    int MinValue,
    int MaxValue, 
    double AverageValue,
    bool IsPet,
    int CombatPlayerId
    );
