namespace CombatParser.Domain.EntityData;

public record UnitAuraData(
    int GameAuraId,
    string Name, 
    int AuraCreatorType,
    int AuraType,
    TimeSpan StartTime,
    TimeSpan FinishTime, 
    int Stacks,
    string TargetGameId
    );
