using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatDataRepository<TModel>
    where TModel : class, ICombatRefs
{
    Task<IEnumerable<TModel>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
