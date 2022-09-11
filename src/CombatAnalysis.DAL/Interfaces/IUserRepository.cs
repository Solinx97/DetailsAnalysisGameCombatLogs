using CombatAnalysis.DAL.Entities.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<string> CreateAsync(AppUser item);

        Task<int> UpdateAsync(AppUser item);

        Task<int> DeleteAsync(AppUser item);

        Task<AppUser> GetByIdAsync(string id);

        Task<AppUser> GetAsync(string email, string password);

        Task<AppUser> GetAsync(string email);

        Task<IEnumerable<AppUser>> GetAllAsync();
    }
}
