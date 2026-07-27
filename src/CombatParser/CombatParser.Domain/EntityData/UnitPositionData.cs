namespace CombatParser.Domain.EntityData;

public record UnitPositionData(
    string GameId,
    int X, 
    int Y,
    TimeSpan Time,
    int CombatId
    );
