using CombatAnalysis.UserDAL.Data;
using CombatAnalysis.UserDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.UserDAL.Services;

internal class ContextService : IContextService
{
    private readonly UserSQLContext _context;
    private IDbContextTransaction _transaction;

    public ContextService(UserSQLContext context)
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
