using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IResetTokenRepository
{
    Task CreateAsync(ResetToken resetCode);

    Task<ResetToken> GetByIdAsync(int id);

    Task UpdateAsync(ResetToken resetCode);

    Task<ResetToken> GetByTokenAsync(string token);
}
