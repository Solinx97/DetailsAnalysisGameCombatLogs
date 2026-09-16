namespace CombatParser.Domain.Entities.Dashboard;

public record Dashboard(
    string Username,
    int Type,
    double AverageDPS,
    double AverageHPS,
    int DeathCount
    );