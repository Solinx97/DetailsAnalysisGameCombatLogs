namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.Dashboard;

public class DashboardModel
{
    public string Username { get; set; } = string.Empty;

    public double AverageDPS { get; set; }

    public double AverageHPS { get; set; }

    public double AverageDeaths { get; set; }
}
