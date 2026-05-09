namespace CombatParser.Domain.EntityData;

public record HealDoneGeneralData(
    int GameSpellId,
    string Spell,
    int Value,
    double HealPerSecond,
    int CritNumber,
    int CastNumber,
    int MinValue,
    int MaxValue,
    double AverageValue,
    int CombatPlayerId
    );
