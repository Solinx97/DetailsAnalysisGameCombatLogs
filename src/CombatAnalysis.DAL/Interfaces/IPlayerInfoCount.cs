using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IPlayerInfoCount<TModel> : IPlayerInfo<TModel>
    where TModel : class, ICombatPlayerEntity
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}