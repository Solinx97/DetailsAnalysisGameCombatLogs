using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerGenericDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}
