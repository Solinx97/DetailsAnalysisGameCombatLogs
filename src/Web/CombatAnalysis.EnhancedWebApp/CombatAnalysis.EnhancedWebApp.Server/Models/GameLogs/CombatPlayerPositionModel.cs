namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatPlayerPositionModel
{
    public string Id { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }

    public int CombatId { get; set; }
}
