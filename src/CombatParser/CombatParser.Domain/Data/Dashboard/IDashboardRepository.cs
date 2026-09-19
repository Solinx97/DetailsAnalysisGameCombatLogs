namespace CombatParser.Domain.Data.Dashboard;

public interface IDashboardRepository
{
    Task<Entities.Dashboard.Dashboard> GetDamageAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken);

    Task<Domain.Entities.Dashboard.Dashboard> GetDamageTakenAsync(int combatLogId, int combatId, string unitName, int valueType, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetDamageSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealSpellsAsync(int combatLogId, int combatId, string unitName, CancellationToken cancellationToken);
}
