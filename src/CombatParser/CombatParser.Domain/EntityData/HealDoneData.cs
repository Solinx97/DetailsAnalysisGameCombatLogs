namespace CombatParser.Domain.EntityData;

public record HealDoneData(
    int GameSpellId,
    string Spell,
    int Value,
    int Overheal,
    TimeSpan Time,
    string Creator,
    string Target,
    bool IsCrit,
    bool IsAbsorbed,
    int CombatPlayerId
    );
