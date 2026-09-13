namespace CombatParser.Domain.EntityData;

public record UnitCastData(
    string OwnerGameId,
    int GameSpellId,
    string Spell,
    TimeSpan Time,
    TimeSpan FinishTime,
    string? TargetGameId,
    bool IsImmediatly,
    bool IsSuccess
    );