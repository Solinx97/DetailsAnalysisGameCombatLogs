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

    public async Task RemoveExpiredTokensAsync()
    {
        await _tokenRepository.RemoveExpiredTokensAsync();
    }
}
