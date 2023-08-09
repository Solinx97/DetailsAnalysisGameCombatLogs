using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

public class SQLSecretRepository : IAppSecret
{
    private readonly SQLContext _context;

    public SQLSecretRepository(SQLContext context)
    {
        _context = context;
    }

    public async Task<Secret> CreateAsync(Secret item)
    {
        var entityEntry = await _context.Set<Secret>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(Secret item)
    {
        _context.Set<Secret>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<Secret> GetByIdAsync(int id)
    {
        var entity = await _context.Set<Secret>().FindAsync(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<Secret>> GetAllAsync() => await _context.Set<Secret>().AsNoTracking().ToListAsync();

    public async Task<int> UpdateAsync(Secret item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
