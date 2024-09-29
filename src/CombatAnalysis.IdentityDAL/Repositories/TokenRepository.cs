using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public TokenRepository(CombatAnalysisIdentityContext dbContext)
    {
        _context = dbContext;
    }

    public async Task SaveAsync(string token, int refreshTokenExpiresDays, string clientId, string userId)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid().ToString(),
            Token = token,
            ExpiryTime = DateTimeOffset.UtcNow.AddDays(refreshTokenExpiresDays),
            ClientId = clientId,
            UserId = userId,
        };

        _context.RefreshToken.Add(refreshToken);

        await _context.SaveChangesAsync();
    }

    public async Task<string> ValidateRefreshTokenAsync(string refreshToken, string clientId)
    {
        var tokenEntry = await _context.RefreshToken
            .FirstOrDefaultAsync(t => t.Token == refreshToken && t.ClientId == clientId);
        if (tokenEntry != null && tokenEntry.ExpiryTime > DateTime.UtcNow)
        {
            return tokenEntry.UserId;
        }

        return null;
    }

    public async Task RemoveExpiredTokensAsync()
    {
        var expiredTokens = _context.RefreshToken.Where(t => t.ExpiryTime < DateTime.UtcNow);
        _context.RefreshToken.RemoveRange(expiredTokens);

        await _context.SaveChangesAsync();
    }
}
