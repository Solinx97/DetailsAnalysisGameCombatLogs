using CombatAnalysis.DAL.Entities.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Interfaces
{
    public interface ITokenRepository
    {
        Task<int> CreateAsync(RefreshToken item);

        Task<int> UpdateAsync(RefreshToken item);

        Task<int> DeleteAsync(RefreshToken item);

        Task<RefreshToken> Get(string token);

        Task<List<RefreshToken>> GetByUser(string userId);
    }
}
