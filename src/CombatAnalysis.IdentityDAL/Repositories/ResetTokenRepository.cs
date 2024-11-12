using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class ResetTokenRepository : IResetTokenRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public ResetTokenRepository(CombatAnalysisIdentityContext dbContext)
    {
        _context = dbContext;
    }

    public async Task CreateAsync(ResetToken resetCode)
    {
        _context.ResetToken.Add(resetCode);
        await _context.SaveChangesAsync();
    }

    public async Task<ResetToken> GetByIdAsync(int id)
    {
        var resetCode = await _context.ResetToken
            .FirstOrDefaultAsync(c => c.Id == id);

        return resetCode;
    }

    public async Task<ResetToken> GetByTokenAsync(string token)
    {
        var resetCode = await _context.ResetToken
            .FirstOrDefaultAsync(c => c.Token == token);

        return resetCode;
    }

    public async Task UpdateAsync(ResetToken resetCode)
    {
        _context.ResetToken.Update(resetCode);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveExpiredResetTokenAsync()
    {
        var tokens = _context.ResetToken.Where(t => t.ExpirationTime < DateTime.UtcNow);
        _context.ResetToken.RemoveRange(tokens);

        await _context.SaveChangesAsync();
    }
}
