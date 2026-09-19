namespace CombatParser.Domain.Data.Dashboard;

public interface IDashboardRepository
{
    Task<Entities.Dashboard.Dashboard> GetDamagePerSecondAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealPerSecondAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetDamageSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);
}
