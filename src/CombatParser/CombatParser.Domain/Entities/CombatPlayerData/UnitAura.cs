using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class UnitAura : CombatPlayerUnitDataBase
{
    public const int NAME_MAX_LENGTH = 128;

    private UnitAura() { }

    private UnitAura(int gameAuraId, string name, int auraCreatorType, int auraType,
        TimeSpan startTime, TimeSpan finishTime, int stacks, string targetGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameAuraId = gameAuraId;
        Name = name;
        AuraCreatorType = auraCreatorType;
        AuraType = auraType;
        StartTime = startTime;
        FinishTime = finishTime;
        Stacks = stacks;
        TargetGameId = targetGameId;
    }

    public int GameAuraId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int AuraCreatorType { get; private set; }

    public int AuraType { get; private set; }

    public TimeSpan StartTime { get; private set; }

    public TimeSpan FinishTime { get; private set; }

    public int Stacks { get; private set; }

    public static UnitAura Create(int gameAuraId, string name, int auraCreatorType, int auraType,
        TimeSpan startTime, TimeSpan finishTime, int stacks, string targetGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));

        return new UnitAura(gameAuraId, name, auraCreatorType, auraType,
            startTime, finishTime, stacks, targetGameId);
    }
}
