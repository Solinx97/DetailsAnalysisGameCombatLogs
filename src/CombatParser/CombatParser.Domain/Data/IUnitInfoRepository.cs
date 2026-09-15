using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data;

public interface IUnitInfoRepository<TModel>
    where TModel : class, ICombatUnitRefs
{
    Task<IEnumerable<TModel>> GetByUnitIdAsync(string unitId, CancellationToken cancellationToken);

    Task<IEnumerable<DamageDoneGeneral>> GetDamageByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<DamageDoneGeneral>> GetDamageTakenByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<HealDoneGeneral>> GetHealByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<ResourceRecoveryGeneral>> GetResourcesByUnitIdAsync(string unitId, int combatId, CancellationToken cancellationToken);

    Task<TModel?> GetFirstByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken);
}