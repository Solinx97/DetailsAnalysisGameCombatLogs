namespace CombatParser.Domain.EntityData;

public record DamageTakenGeneralData(
    int GameSpellId,
    string Spell,
    int Value,
    int ActualValue,
    double DamageTakenPerSecond,
    int MissNumber,
    int CritNumber,
    int CastNumber,
    int MinValue,
    int MaxValue,
    double AverageValue,
    int CombatPlayerId
    );
