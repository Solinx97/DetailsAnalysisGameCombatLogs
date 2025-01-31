namespace CombatAnalysis.BL.Interfaces.General;

public interface IMutationService<TModel>
    where TModel : class
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TModel item);
}
