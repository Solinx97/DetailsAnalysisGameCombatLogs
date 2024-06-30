using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.IdentityDAL.Interfaces;

public interface IPkeRepository
{
    Task SaveCodeChallengeAsync(string clientId, string authorizationCode, string codeChallenge, string codeChallengeMethod);

    AuthorizationCodeChallenge GetCodeChallenge(string authorizationCode);
}
