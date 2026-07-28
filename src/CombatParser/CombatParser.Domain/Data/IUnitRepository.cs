using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface IUnitRepository<TModel>
    where TModel : class, ICombatRefs, IUnitRef, ITime
{
    Task<IDictionary<string, IEnumerable<TModel>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
