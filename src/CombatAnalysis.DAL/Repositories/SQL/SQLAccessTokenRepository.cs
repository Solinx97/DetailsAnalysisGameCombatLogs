using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

public class SQLAccessTokenRepository : ITokenRepository<AccessToken>
{
    private readonly SQLContext _context;

    public SQLAccessTokenRepository(SQLContext context)
    {
        _context = context;
    }

    public async Task<AccessToken> CreateAsync(AccessToken item)
    {
        var entityEntry = await _context.Set<AccessToken>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(AccessToken item)
    {
        _context.Set<AccessToken>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<AccessToken> GetByTokenAsync(string token)
    {
        var allTokens = await _context.Set<AccessToken>().AsNoTracking().ToListAsync();
        if (!allTokens.Any())
        {
            return null;
        }

        var foundToken = allTokens.Find(AccessToken => AccessToken.Token == token);
        return foundToken;
    }

    public async Task<IEnumerable<AccessToken>> GetAllByUserAsync(string userId)
    {
        var allTokens = await _context.Set<AccessToken>().AsNoTracking().ToListAsync();
        if (!allTokens.Any())
        {
            return null;
        }

        var foundTokens = allTokens.FindAll(AccessToken => AccessToken.UserId == userId);
        return foundTokens;
    }

    public async Task<int> UpdateAsync(AccessToken item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
