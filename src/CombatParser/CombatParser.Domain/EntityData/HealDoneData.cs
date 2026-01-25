namespace CombatParser.Domain.EntityData;

public record HealDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    TimeSpan Time,
    string Creator,
    string Target,
    int Overheal,
    bool IsCrit,
    bool IsAbsorbed,
    int CombatPlayerId
    );
