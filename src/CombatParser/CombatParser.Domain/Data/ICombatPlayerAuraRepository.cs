using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerAuraRepository
{
    Task<IEnumerable<UnitAura>> GetAurasAsync(int combatId, CancellationToken cancellationToke);

    Task<IEnumerable<UnitAura>> GetAurasAsync(string unitId, CancellationToken cancellationToken);

    Task<UnitAura?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
