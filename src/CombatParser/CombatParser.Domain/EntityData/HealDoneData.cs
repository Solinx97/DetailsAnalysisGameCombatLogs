namespace CombatParser.Domain.EntityData;

public record HealDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    int Overheal,
    TimeSpan Time,
    string CreatorId,
    string CreatorGameId,
    string TargetId,
    string TargetGameId,
    int ModificationType
    );
