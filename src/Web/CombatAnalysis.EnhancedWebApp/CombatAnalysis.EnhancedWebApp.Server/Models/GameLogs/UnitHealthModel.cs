namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class UnitHealthModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public long CurrentHealth { get; set; }

    public long MaxHealth { get; set; }

    public int Status { get; set; }

    public TimeSpan Time { get; set; }

    public string CombatUnitId { get; set; } = string.Empty;
}
