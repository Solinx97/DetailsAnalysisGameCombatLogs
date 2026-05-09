using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken);

    Task<int> CountAsync(int combatPlayerId, CancellationToken cancellationToken);
}
