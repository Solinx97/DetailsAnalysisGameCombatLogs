using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class IdentityUserRepository : IIdentityUserRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public IdentityUserRepository(CombatAnalysisIdentityContext dbContext)
    {
        _context = dbContext;
    }

    public async Task SaveAsync(IdentityUser identityUser)
    {
        _context.IdentityUser.Add(identityUser);
        await _context.SaveChangesAsync();
    }

    public IdentityUser Get(string id)
    {
        var identityUser = _context.IdentityUser
            .FirstOrDefault(c => c.Id == id);

        return identityUser;
    }

    public async Task<IdentityUser> GetAsync(string email, string password)
    {
        var entity = await _context.Set<IdentityUser>().FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }
}
