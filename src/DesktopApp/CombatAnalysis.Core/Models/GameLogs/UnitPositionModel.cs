namespace CombatAnalysis.Core.Models.GameLogs;

public class UnitPositionModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    public TimeSpan Time { get; set; }

    public string UnitId { get; set; }
}
