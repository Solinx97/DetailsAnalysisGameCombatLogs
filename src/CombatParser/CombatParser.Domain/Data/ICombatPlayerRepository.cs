using CombatParser.Domain.Entities;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerRepository
{
    Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);

    Task<CombatPlayer?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IPlayerStats?> GetPlayerStatsAsync(int combatPlayerId, int gameVersion, CancellationToken cancellationToken);
}
