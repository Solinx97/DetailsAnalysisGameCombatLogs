namespace CombatParser.Domain.Entities.Dashboard;

public class DashboardQuery<TModel>
    where TModel : class
{
    public Unit Unit { get; set; }

    public TModel Value { get; set; }
}
