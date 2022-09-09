using CombatAnalysis.DAL.Entities.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<string> CreateAsync(User item);

        Task<int> UpdateAsync(User item);

        Task<int> DeleteAsync(User item);

        Task<User> GetByIdAsync(string id);

        Task<User> GetAsync(string email, string password);

        Task<User> GetAsync(string email);

        Task<IEnumerable<User>> GetAllAsync();
    }
}
