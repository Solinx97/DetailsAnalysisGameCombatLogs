namespace CombatParser.Domain.EntityData;

public record CombatAuraData(
    int GameAuraId,
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
