namespace CombatParser.Domain.EntityData;

public record CombatPlayerCastData(
    int GameSpellId,
    string Spell,
    TimeSpan? StartTime,
    TimeSpan FinishTime,
    string Creator,
    string Target,
    bool IsSuccess,
    int CombatPlayerId
    );