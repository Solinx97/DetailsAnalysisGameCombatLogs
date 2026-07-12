namespace CombatParser.Domain.Entities.Dashboard;

public record Dashboard(
    string Username,
    double AverageDPS,
    double AverageHPS, 
    double AverageDeaths
    );