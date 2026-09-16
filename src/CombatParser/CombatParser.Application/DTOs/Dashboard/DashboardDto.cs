namespace CombatParser.Application.DTOs.Dashboard;

public class DashboardDto
{
    public string Username { get; set; } = string.Empty;

    public int Type { get; set; }

    public double AverageDPS { get; set; }

    public double AverageHPS { get; set; }

    public int DeathCount { get; set; }
}
