using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

public class SQLTokenRepository : ITokenRepository
{
    private readonly SQLContext _context;

    public SQLTokenRepository(SQLContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken item)
    {
        var entityEntry = await _context.Set<RefreshToken>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(RefreshToken item)
    {
        _context.Set<RefreshToken>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
        if (!allTokens.Any())
        {
            return null;
        }

        var foundToken = allTokens.Find(refreshToken => refreshToken.Token == token);
        return foundToken;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllByUserAsync(string userId)
    {
        var allTokens = await _context.Set<RefreshToken>().AsNoTracking().ToListAsync();
        if (!allTokens.Any())
        {
            return null;
        }

        var foundTokens = allTokens.FindAll(refreshToken => refreshToken.UserId == userId);
        return foundTokens;
    }

    public async Task<int> UpdateAsync(RefreshToken item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
