namespace CombatAnalysis.BL.Interfaces;

public interface ISpecScoreService<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetBySpecIdAsync(int specId, int bossId, int difficult);
}
