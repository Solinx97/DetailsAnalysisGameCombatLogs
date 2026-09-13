namespace CombatParser.Domain.EntityData;

public record UnitPositionData(
    string OwnerGameId,
    int X, 
    int Y,
    TimeSpan Time
    );
