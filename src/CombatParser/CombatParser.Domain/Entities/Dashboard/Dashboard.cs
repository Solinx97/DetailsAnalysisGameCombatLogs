namespace CombatParser.Domain.Entities.Dashboard;

public record Dashboard(
    int Type,
    List<DashboardItem> Items
    );