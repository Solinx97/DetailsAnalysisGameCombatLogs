using CombatAnalysis.DAL.Entities.Authentication;

namespace CombatAnalysis.DAL.Interfaces;

public interface IAppSecret
{
    Task<Secret> CreateAsync(Secret item);

    Task<int> DeleteAsync(Secret item);

    Task<int> UpdateAsync(Secret item);

    Task<IEnumerable<Secret>> GetAllAsync();

    Task<Secret> GetByIdAsync(int id);
}
