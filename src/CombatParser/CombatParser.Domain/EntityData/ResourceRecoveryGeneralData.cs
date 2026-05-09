namespace CombatParser.Domain.EntityData;

public record ResourceRecoveryGeneralData(
    int GameSpellId,
    string Spell,
    int Value,
    int ResourcePerSecond,
    int CastNumber,
    int MinValue,
    int MaxValue,
    double AverageValue,
    int CombatPlayerId
    );
