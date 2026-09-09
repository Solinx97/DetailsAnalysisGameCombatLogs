using CombatParser.Domain.Entities;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface IUnitRepository<TModel>
    where TModel : class, ICombatRefs, IUnitRef, ITime
{
    Task<IDictionary<string, IEnumerable<TModel>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);

    Task<IDictionary<string, List<UnitHealth>>> GetHealthByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
