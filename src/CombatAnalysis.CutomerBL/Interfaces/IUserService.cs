namespace CombatAnalysis.CustomerBL.Interfaces;

public interface IUserService<TModel>
    where TModel : class
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TModel item);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> GetByIdAsync(string id);

    Task<TModel> GetAsync(string emil);
}
