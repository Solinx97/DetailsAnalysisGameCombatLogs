namespace CombatAnalysis.BL.Interfaces.General;

public interface IQueryService<TModel>
    where TModel : class
{
    Task<IEnumerable<TModel>> GetAllAsync();

    Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value);

    Task<TModel> GetByIdAsync(int id);
}
