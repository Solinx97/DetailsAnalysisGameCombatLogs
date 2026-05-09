namespace CombatParser.Domain.EntityData;

public record CombatAuraData(
    string Name, 
    string Creator,
    string Target,
    int AuraCreatorType,
    int AuraType,
    TimeSpan StartTime,
    TimeSpan FinishTime, 
    int Stacks,
    int CombatId
    );
