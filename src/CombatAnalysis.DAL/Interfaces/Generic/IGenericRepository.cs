using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Interfaces.Generic;

public interface IGenericRepository<TModel>
    where TModel : class, IEntity
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TModel item);

    Task<TModel> GetByIdAsync(int id);

    Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value);

    Task<IEnumerable<TModel>> GetAllAsync();
}