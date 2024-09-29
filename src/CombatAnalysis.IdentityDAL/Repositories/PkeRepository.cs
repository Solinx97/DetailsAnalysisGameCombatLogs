using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class PkeRepository : IPkeRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public PkeRepository(CombatAnalysisIdentityContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(string clientId, string authorizationCode, string codeChallenge, string codeChallengeMethod, string redirectUri, int expiryTimeMins = 5)
    {
        var entry = new AuthorizationCodeChallenge
        {
            Id = authorizationCode,
            CodeChallenge = codeChallenge,
            ClientId = clientId,
            CodeChallengeMethod = codeChallengeMethod,
            CreatedAt = DateTime.UtcNow,
            ExpiryTime = DateTime.UtcNow.AddMinutes(expiryTimeMins),
            RedirectUrl = redirectUri,
            IsUsed = false
        };

        _context.AuthorizationCodeChallenge.Add(entry);

        await _context.SaveChangesAsync();
    }

    public async Task<AuthorizationCodeChallenge> GetByIdAsync(string id)
    {
        var codeChallenge = await _context.AuthorizationCodeChallenge
            .FirstOrDefaultAsync(c => c.Id == id);

        return codeChallenge;
    }

    public async Task<AuthorizationCodeChallenge> MarkCodeAsUsed(string id)
    {
        var codeChallenge = await _context.AuthorizationCodeChallenge
            .FirstOrDefaultAsync(c => c.Id == id);

        return codeChallenge;
    }

    public async Task<int> MarkCodeAsUsedAsync(AuthorizationCodeChallenge code)
    {
        _context.Entry(code).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task RemoveExpiredCodesAsync()
    {
        var expiredCodes = _context.AuthorizationCodeChallenge.Where(t => t.ExpiryTime < DateTime.UtcNow);
        _context.AuthorizationCodeChallenge.RemoveRange(expiredCodes);

        await _context.SaveChangesAsync();
    }
}
