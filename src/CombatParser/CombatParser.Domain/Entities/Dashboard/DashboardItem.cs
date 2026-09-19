namespace CombatParser.Domain.Entities.Dashboard;

public record DashboardItem(
    string ValueName,
    string Value,
    int UnitType
    );
