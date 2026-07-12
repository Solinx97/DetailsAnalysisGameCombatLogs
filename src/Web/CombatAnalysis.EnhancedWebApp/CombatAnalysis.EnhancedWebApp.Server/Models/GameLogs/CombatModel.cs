namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatModel
{
    public int Id { get; set; }

    public string DungeonName { get; set; } = string.Empty;

    public double BossHealthPercentage { get; set; }

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public int CombatLogId { get; set; }

    public BossModel Boss { get; set; } = new();
}
