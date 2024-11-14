using CombatAnalysis.CustomerDAL.Data;
using CombatAnalysis.CustomerDAL.Entities;
using CombatAnalysis.CustomerDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CustomerDAL.Repositories.SQL;

internal class SQLUserRepository : IUserRepository
{
    private readonly CustomerSQLContext _context;

    public SQLUserRepository(CustomerSQLContext context)
    {
        _context = context;
    }

    public async Task<AppUser> CreateAsync(AppUser item)
    {
        var entityEntry = await _context.Set<AppUser>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(AppUser item)
    {
        _context.Set<AppUser>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync() => await _context.Set<AppUser>().AsNoTracking().ToListAsync();

    public async Task<AppUser> GetByIdAsync(string id)
    {
        var entity = await _context.Set<AppUser>().FindAsync(id);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<AppUser> GetAsync(string identityUserId)
    {
        var entity = await _context.Set<AppUser>().FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<AppUser> UpdateAsync(AppUser item)
    {
        var entity = _context.Update(item);
        await _context.SaveChangesAsync();

        return entity.Entity;
    }
}
