using CombatAnalysis.CustomerDAL.Entities;

namespace CombatAnalysis.CustomerDAL.Interfaces;

public interface IUserRepository
{
    Task<AppUser> CreateAsync(AppUser item);

    Task<AppUser> UpdateAsync(AppUser item);

    Task<int> DeleteAsync(AppUser item);

    Task<AppUser> GetByIdAsync(string id);

    Task<AppUser> GetAsync(string identityUserId);

    Task<IEnumerable<AppUser>> GetAllAsync();
}
