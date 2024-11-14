using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class IdentityTransactionService : IIdentityTransactionService
{
    private readonly IContextService _context;

    public IdentityTransactionService(IContextService context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await _context.BeginAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.RollbackAsync();
    }
}
