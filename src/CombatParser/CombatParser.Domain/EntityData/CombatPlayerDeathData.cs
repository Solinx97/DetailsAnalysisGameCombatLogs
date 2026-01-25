namespace CombatParser.Domain.EntityData;

public record CombatPlayerDeathData(
    string Username,
    string LastHitSpell,
    int LastHitValue, 
    TimeSpan Time,
    int CombatPlayerId
    );
