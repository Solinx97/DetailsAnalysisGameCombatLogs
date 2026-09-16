namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class UnitPreAuraModel
{
    public string Id { get; set; }

    public string TargetGameId { get; set; } = string.Empty;

    public int GameId { get; set; }

    public int Status { get; set; }

    public string UnitId { get; set; }
}
