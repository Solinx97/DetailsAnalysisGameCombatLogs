using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface ICountRepository<TModel>
    where TModel : class, IEntity
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}