namespace CombatParser.Application.DTOs.Dashboard;

public class DashboardDto
{
    public string Username { get; set; } = string.Empty;

    public double AverageDPS { get; set; }

    public double AverageHPS { get; set; }

    public double AverageDeaths { get; set; }
}
