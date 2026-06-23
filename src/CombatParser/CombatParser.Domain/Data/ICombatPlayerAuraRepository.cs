using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerAuraRepository
{
    Task<IEnumerable<CombatPlayerAura>> GetAurasAsync(int combatId, CancellationToken cancellationToke);

    Task<IEnumerable<CombatPlayerAura>> GetAurasAsync(int combatId, int combatPlayerId, CancellationToken cancellationToken);

    Task<CombatPlayerAura?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
