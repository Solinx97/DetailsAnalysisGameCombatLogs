using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerDataByTimeRepository<TModel>
    where TModel : class, ICombatPlayerRefs, ITime
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}
