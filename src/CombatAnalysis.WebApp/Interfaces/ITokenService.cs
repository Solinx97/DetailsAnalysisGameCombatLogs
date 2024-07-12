using CombatAnalysis.WebApp.Models.Identity;

namespace CombatAnalysis.WebApp.Interfaces;

public interface ITokenService
{
    Task<AccessTokenModel> RefreshAccessTokenAsync(string refreshToken);

    bool IsAccessTokenCloseToExpiry(string accessToken);
}
