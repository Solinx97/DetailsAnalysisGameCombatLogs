namespace CombatAnalysis.BL.Interfaces;

public interface IPlayerInfoService<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(TIdType combatPlayerId);
}