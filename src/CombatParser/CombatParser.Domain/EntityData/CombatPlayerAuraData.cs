namespace CombatParser.Domain.EntityData;

public record CombatPlayerAuraData(
    int GameAuraId,
    string Name, 
    string Creator,
    string Target,
    int AuraCreatorType,
    int AuraType,
    TimeSpan StartTime,
    TimeSpan FinishTime, 
    int Stacks,
    int CombatPlayerId
    );
