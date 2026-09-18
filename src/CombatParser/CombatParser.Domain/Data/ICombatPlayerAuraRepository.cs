using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerAuraRepository
{
    Task<IEnumerable<UnitAura>> GetAurasAsync(int combatId, CancellationToken cancellationToke);

    Task<UnitAura?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
