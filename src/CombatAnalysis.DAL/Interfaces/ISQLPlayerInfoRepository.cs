namespace CombatAnalysis.DAL.Interfaces;

public interface ISQLPlayerInfoRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(TIdType combatPlayerId);
}