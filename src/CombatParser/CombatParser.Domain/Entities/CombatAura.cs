using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class CombatAura : CombatDataBase, ICombatData
{
    public const int NAME_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private CombatAura() { }

    public CombatAura(string name, string creator, string target, int auraCreatorType, int auraType,
        TimeSpan startTime, TimeSpan finishTime, int stacks, int combatId)
    {
        Name = name;
        Creator = creator;
        Target = target;
        AuraCreatorType = auraCreatorType;
        AuraType = auraType;
        StartTime = startTime;
        FinishTime = finishTime;
        Stacks = stacks;
        CombatId = combatId;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Creator { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public int AuraCreatorType { get; private set; }

    public int AuraType { get; private set; }

    public TimeSpan StartTime { get; private set; }

    public TimeSpan FinishTime { get; private set; }

    public int Stacks { get; private set; }

    public Combat Combat { get; private set; }
}
