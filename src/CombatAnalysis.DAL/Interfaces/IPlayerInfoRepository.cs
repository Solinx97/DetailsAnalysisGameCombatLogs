using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerInfoRepository<TModel>
    where TModel : class, IEntity
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId);

    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize);
}