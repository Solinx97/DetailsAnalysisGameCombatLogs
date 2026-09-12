namespace CombatParser.Domain.EntityData;

public record ResourceRecoveryData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string CreatorId,
    string CreatorGameId,
    string TargetId,
    string TargetGameId,
    int ModificationType,
    int CombatPlayerId
    );
