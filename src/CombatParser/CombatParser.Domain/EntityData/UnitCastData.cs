namespace CombatParser.Domain.EntityData;

public record UnitCastData(
    int GameSpellId,
    string Spell,
    TimeSpan Time,
    TimeSpan FinishTime,
    string CreatorGameId,
    string? TargetGameId,
    bool IsImmediatly,
    bool IsSuccess,
    int CombatId
    );