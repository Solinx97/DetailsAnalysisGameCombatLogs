using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Data.SQL;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.BL.Services;

internal class SqlContextService : ISqlContextService
{
    private readonly SQLContext _context;

    public SqlContextService(SQLContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
