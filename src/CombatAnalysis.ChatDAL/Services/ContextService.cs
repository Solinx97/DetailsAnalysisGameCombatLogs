using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.ChatDAL.Services;

internal class ContextService : IContextService
{
    private readonly ChatSQLContext _context;
    private IDbContextTransaction? _transaction;

    public ContextService(ChatSQLContext context)
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
