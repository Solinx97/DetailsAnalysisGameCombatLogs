namespace CombatParser.Domain.Data.Dashboard;

public interface IDashboardRepository
{
    Task<Entities.Dashboard.Dashboard[]> GetAsync(int combatLogId, CancellationToken cancellationToken);
}
