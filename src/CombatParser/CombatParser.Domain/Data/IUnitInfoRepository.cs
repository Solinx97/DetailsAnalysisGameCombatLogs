using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface IUnitInfoRepository<TModel>
    where TModel : class, ICombatUnitRefs
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken);

    Task<IEnumerable<DamageDoneGeneral>> GetDamageByCombatPlayerIdAsync(string unitId, bool isPlayerTarget, CancellationToken cancellationToken);

    Task<TModel?> GetFirstByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken);
}