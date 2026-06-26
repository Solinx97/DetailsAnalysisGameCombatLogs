namespace CombatParser.Domain.EntityData;

public record CombatPlayerPositionData(
    int X, 
    int Y,
    TimeSpan Time,
    int CombatPlayerId
    );
