namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatLogModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public int LogType { get; set; }

    public string AppUserId { get; set; }
}
