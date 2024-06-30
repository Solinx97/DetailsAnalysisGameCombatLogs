using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class PkeRepository : IPkeRepository
{
    private readonly CombatAnalysisIdentityContext _dbContext;

    public PkeRepository(CombatAnalysisIdentityContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveCodeChallengeAsync(string clientId, string authorizationCode, string codeChallenge, string codeChallengeMethod)
    {
        var entry = new AuthorizationCodeChallenge
        {
            Id = authorizationCode,
            CodeChallenge = codeChallenge,
            RequestId = string.Empty,
            SessionId = string.Empty,
            ClientId = clientId,
            CodeChallengeMethod = codeChallengeMethod,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AuthorizationCodeChallenge.Add(entry);
        await _dbContext.SaveChangesAsync();
    }

    public AuthorizationCodeChallenge GetCodeChallenge(string authorizationCode)
    {
        var condeChallenge = _dbContext.AuthorizationCodeChallenge
            .FirstOrDefault(c => c.Id == authorizationCode);

        return condeChallenge;
    }
}
