using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.CommunicationBL.Services;

internal class SqlContextService : ISqlContextService
{
    private readonly CommunicationSQLContext _context;
    private IDbContextTransaction _transaction;

    public SqlContextService(CommunicationSQLContext context)
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
