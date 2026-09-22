namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class UnitAura
{
    public int GameAuraId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string TargetGameId { get; set; } = string.Empty;

    public int AuraCreatorType { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }
}
