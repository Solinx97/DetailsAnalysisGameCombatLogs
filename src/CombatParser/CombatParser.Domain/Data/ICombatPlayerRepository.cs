using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerRepository
{
    Task<IEnumerable<string>> GetUniquePlayerNames(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);

    Task<CombatPlayer?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IPlayerStats?> GetPlayerStatsAsync(int combatPlayerId, int gameVersion, CancellationToken cancellationToken);

    Task<int> GetPlayerDeathCountAsync(string unitId, CancellationToken cancellationToken);

    Task<List<CombatPlayerDeath>> GetPlayerDeathAsync(string unitId, int skipCount, CancellationToken cancellationToken);
}
