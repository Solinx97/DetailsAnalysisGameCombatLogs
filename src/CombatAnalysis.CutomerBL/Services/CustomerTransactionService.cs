using CombatAnalysis.CustomerBL.Interfaces;
using CombatAnalysis.CustomerDAL.Interfaces;

namespace CombatAnalysis.CustomerBL.Services;

internal class CustomerTransactionService : ICustomerTransactionService
{
    private readonly IContextService _context;

    public CustomerTransactionService(IContextService context)
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
