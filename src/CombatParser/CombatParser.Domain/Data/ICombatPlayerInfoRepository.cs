using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface ICombatPlayerInfoRepository<TModel>
    where TModel : class, ICombatPlayerRefs
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<IEnumerable<DamageDoneGeneral>> GetDamageByCombatPlayerIdAsync(int combatPlayerId, bool isPlayerTarget, CancellationToken cancellationToken);

    Task<TModel?> GetFirstByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken);
}