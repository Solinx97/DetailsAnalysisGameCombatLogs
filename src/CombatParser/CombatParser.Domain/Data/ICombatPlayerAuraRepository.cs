using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerAuraRepository
{
    Task<IEnumerable<CombatPlayerAura>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToke);

    Task<CombatPlayerAura?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
