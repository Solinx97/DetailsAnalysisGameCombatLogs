using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs, ICombatPlayerTime
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}
