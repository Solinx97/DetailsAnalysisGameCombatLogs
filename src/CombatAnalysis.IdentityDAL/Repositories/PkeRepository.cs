using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class PkeRepository : IPkeRepository
{
    private readonly CombatAnalysisIdentityContext _dbContext;

    public PkeRepository(CombatAnalysisIdentityContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(string clientId, string authorizationCode, string codeChallenge, string codeChallengeMethod)
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

    public async Task<AuthorizationCodeChallenge> GetByIdAsync(string id)
    {
        var condeChallenge = await _dbContext.AuthorizationCodeChallenge
            .FirstOrDefaultAsync(c => c.Id == id);

        return condeChallenge;
    }
}
