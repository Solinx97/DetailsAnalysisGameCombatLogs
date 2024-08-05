namespace CombatAnalysis.DAL.Interfaces;

public interface ISpecScore<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetBySpecIdAsync(int specId, int bossId, int difficult);
}
