using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerPositionRepository
{
    Task<IEnumerable<CombatPlayerPosition>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
