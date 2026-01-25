namespace CombatParser.Domain.EntityData;

public record CombatPlayerPositionData(
    int PositionX, 
    int PositionY,
    TimeSpan Time,
    int CombatPlayerId,
    int CombatId
    );
