namespace CombatParser.Domain.EntityData;

public record UnitPositionData(
    string CreatorGameId,
    int X, 
    int Y,
    TimeSpan Time,
    int CombatId
    );
