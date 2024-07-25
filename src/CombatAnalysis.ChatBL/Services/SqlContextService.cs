using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.ChatBL.Services;

internal class SqlContextService : ISqlContextService
{
    private readonly ChatSQLContext _context;
    private IDbContextTransaction _transaction;

    public SqlContextService(ChatSQLContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(bool createSharedTransaction)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        if (createSharedTransaction)
        {
            _transaction = transaction;
        }

        return transaction;
    }

    public async Task<IDbContextTransaction> UseTransactionAsync()
    {
        if (_transaction == null)
        {
            return await _context.Database.BeginTransactionAsync();
        }
        else
        {
            return await _context.Database.UseTransactionAsync(_transaction.GetDbTransaction());
        }
    }
}
