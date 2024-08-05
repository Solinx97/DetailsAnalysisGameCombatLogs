using CombatAnalysis.DAL.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerInfoCount<TModel, TIdType> : IPlayerInfo<TModel, TIdType>
    where TModel : BasePlayerInfo
    where TIdType : notnull
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}
