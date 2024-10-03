using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class TokenService : ITokenService
{
    private readonly ITokenRepository _tokenRepository;

    public TokenService(ITokenRepository tokenRepository)
    {
        _tokenRepository = tokenRepository;
    }

    public void RemoveExpiredTokens()
    {
        _tokenRepository.RemoveExpiredTokensAsync().Wait();
    }
}
