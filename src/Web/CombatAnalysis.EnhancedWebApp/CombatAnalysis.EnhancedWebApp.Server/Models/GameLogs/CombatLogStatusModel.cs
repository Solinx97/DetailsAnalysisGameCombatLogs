namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatLogStatusModel
{
    public int Id { get; set; }

    public DateTimeOffset Date { get; set; }

    public int Status { get; set; }

    public int CombatLogId { get; set; }
}
