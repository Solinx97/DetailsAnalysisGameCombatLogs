using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class VerifyEmailTokenRepository : IVerifyEmailTokenRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public VerifyEmailTokenRepository(CombatAnalysisIdentityContext dbContext)
    {
        _context = dbContext;
    }

    public async Task CreateAsync(VerifyEmailToken verifyCode)
    {
        _context.VerifyEmailToken.Add(verifyCode);
        await _context.SaveChangesAsync();
    }

    public async Task<VerifyEmailToken> GetByIdAsync(int id)
    {
        var resetCode = await _context.VerifyEmailToken
            .FirstOrDefaultAsync(c => c.Id == id);

        return resetCode;
    }

    public async Task<VerifyEmailToken> GetByTokenAsync(string token)
    {
        var resetCode = await _context.VerifyEmailToken
            .FirstOrDefaultAsync(c => c.Token == token);

        return resetCode;
    }

    public async Task UpdateAsync(VerifyEmailToken verifyCode)
    {
        _context.VerifyEmailToken.Update(verifyCode);
        await _context.SaveChangesAsync();
    }
}
