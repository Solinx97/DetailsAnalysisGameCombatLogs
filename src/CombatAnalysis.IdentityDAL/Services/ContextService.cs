using CombatAnalysis.IdentityDAL.Data;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.IdentityDAL.Services;

internal class ContextService : IContextService
{
    private readonly CombatAnalysisIdentityContext _context;
    private IDbContextTransaction _transaction;

    public ContextService(CombatAnalysisIdentityContext context)
    {
        _context = context;
    }

    public async Task BeginAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
