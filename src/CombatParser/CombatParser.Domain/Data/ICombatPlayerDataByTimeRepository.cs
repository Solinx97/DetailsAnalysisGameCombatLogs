using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerDataByTimeRepository<TModel>
    where TModel : class, ICombatUnitRefs, ITime
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken);
}
