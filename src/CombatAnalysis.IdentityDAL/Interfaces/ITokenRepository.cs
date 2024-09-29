namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface ITokenRepository
{
    Task SaveAsync(string token, int refreshTokenExpiresDays, string clientId, string userId);

    Task<string> ValidateRefreshTokenAsync(string refreshToken, string clientId);

    Task RemoveExpiredTokensAsync();
}
