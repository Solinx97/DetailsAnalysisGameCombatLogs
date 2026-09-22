namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class UnitAuraModel
{
    public string Id { get; set; }

    public int GameAuraId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string TargetGameId { get; set; } = string.Empty;

    public int AuraCreatorType { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }

    public string UnitId { get; set; }
}
