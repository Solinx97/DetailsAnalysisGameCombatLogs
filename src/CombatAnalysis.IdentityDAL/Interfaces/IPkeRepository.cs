using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IPkeRepository
{
    Task CreateAsync(string clientId, string authorizationCode, string codeChallenge, string codeChallengeMethod, string redirectUri);

    Task<AuthorizationCodeChallenge> GetByIdAsync(string id);

    Task<int> MarkCodeAsUsedAsync(AuthorizationCodeChallenge code);
}
