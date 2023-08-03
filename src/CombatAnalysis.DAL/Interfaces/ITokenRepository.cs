using CombatAnalysis.DAL.Entities.Authentication;

namespace CombatAnalysis.DAL.Interfaces;

public interface ITokenRepository<TModel>
    where TModel : class
{
    Task<TModel> CreateAsync(TModel item);

    Task<int> UpdateAsync(TModel item);

    Task<int> DeleteAsync(TModel item);

    Task<TModel> GetByTokenAsync(string token);

    Task<IEnumerable<TModel>> GetAllByUserAsync(string userId);
}
