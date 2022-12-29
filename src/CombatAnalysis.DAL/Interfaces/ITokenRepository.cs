using CombatAnalysis.DAL.Entities.Authentication;

namespace CombatAnalysis.DAL.Interfaces;

public interface ITokenRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken item);

    Task<int> UpdateAsync(RefreshToken item);

    Task<int> DeleteAsync(RefreshToken item);

    Task<RefreshToken> GetByTokenAsync(string token);

    Task<IEnumerable<RefreshToken>> GetAllByUserAsync(string userId);
}
