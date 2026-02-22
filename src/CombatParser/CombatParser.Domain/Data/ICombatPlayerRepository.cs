using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerRepository
{
    Task<IEnumerable<CombatPlayer>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);

    Task<CombatPlayer?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
