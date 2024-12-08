namespace CombatAnalysis.BL.Interfaces;

public interface IService<TModel>
    where TModel : class
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(int id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value);

    Task<TModel> GetByIdAsync(int id);
}
