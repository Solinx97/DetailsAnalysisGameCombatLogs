using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IVerifyEmailTokenRepository
{
    Task CreateAsync(VerifyEmailToken verifyCode);

    Task<VerifyEmailToken> GetByIdAsync(int id);

    Task UpdateAsync(VerifyEmailToken verifyCode);

    Task<VerifyEmailToken> GetByTokenAsync(string token);

    Task RemoveExpiredVerifyEmailTokenAsync();
}
