using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatDataRepository<TModel>
    where TModel : class, ICombatRefs
{
    Task<IEnumerable<TTimeModel>> GetByCombatIdAsync<TTimeModel>(int combatId, CancellationToken cancellationToken) where TTimeModel : class, ICombatRefs, ICombatTime;

    Task<IEnumerable<TModel>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken);
}
