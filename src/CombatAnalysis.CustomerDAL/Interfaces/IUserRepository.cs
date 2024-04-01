using CombatAnalysis.CustomerDAL.Entities;

namespace CombatAnalysis.CustomerDAL.Interfaces;

public interface IUserRepository
{
    Task<AppUser> CreateAsync(AppUser item);

    Task<int> UpdateAsync(AppUser item);

    Task<int> DeleteAsync(AppUser item);

    Task<AppUser> GetByIdAsync(string id);

    Task<AppUser> GetAsync(string email, string password);

    Task<AppUser> GetAsync(string email);

    Task<IEnumerable<AppUser>> GetAllAsync();
}
