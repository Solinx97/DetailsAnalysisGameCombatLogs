using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.IdentityDAL.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly CombatAnalysisIdentityContext _context;

    public ClientRepository(CombatAnalysisIdentityContext dbContext)
    {
        _context = dbContext;
    }

    public async Task SaveAsync(Client identityUser)
    {
        _context.Client.Add(identityUser);

        await _context.SaveChangesAsync();
    }

    public async Task<Client> GetByIdAsync(string id)
    {
        var identityUser = await _context.Client
            .FirstOrDefaultAsync(c => c.Id == id);

        return identityUser;
    }
}
