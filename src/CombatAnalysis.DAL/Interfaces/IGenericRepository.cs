using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces;

public interface IGenericRepository<TModel>
    where TModel : class, IEntity
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(int id);

    Task<TModel> GetByIdAsync(int id);

    IEnumerable<TModel> GetByParam(string paramName, object value);

    Task<IEnumerable<TModel>> GetAllAsync();
}