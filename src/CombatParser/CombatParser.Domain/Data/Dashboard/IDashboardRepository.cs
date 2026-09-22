namespace CombatParser.Domain.Data.Dashboard;

public interface IDashboardRepository
{
    Task<Entities.Dashboard.Dashboard> GetDamageAsync(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealAsync(int combatLogId, string bossName, int combatId, string creatorName, string targetName, int valueType, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetDamageSpellsAsync(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken);

    Task<Entities.Dashboard.Dashboard> GetHealSpellsAsync(int combatLogId, string bossName, int combatId, CancellationToken cancellationToken);
}
