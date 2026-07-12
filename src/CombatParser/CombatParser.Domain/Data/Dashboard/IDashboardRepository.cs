namespace CombatParser.Domain.Data.Dashboard;

public interface IDashboardRepository
{
    Task<Entities.Dashboard.Dashboard[]> GetAsync(int combatLogId, CancellationToken cancellationToken);

    Task<Dictionary<string, int>> GetDamageSpellsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<Dictionary<string, int>> GetHealSpellsAsync(int combatLogId, CancellationToken cancellationToken);
}
