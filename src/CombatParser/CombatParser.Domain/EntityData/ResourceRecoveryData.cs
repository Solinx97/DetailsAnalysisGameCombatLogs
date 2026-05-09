namespace CombatParser.Domain.EntityData;

public record ResourceRecoveryData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string Creator,
    string Target,
    int CombatPlayerId
    );
