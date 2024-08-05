using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerInfo<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId);

    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize);
}