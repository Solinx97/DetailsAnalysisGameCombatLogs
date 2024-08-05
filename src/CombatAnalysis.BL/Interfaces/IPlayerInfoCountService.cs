namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoCountService<TModel, TIdType> : IPlayerInfoService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<int> CountByCombatPlayerIdAsync(int combatPlayerId);
}
