using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.BL.Interfaces;

public interface ISqlContextService
{
    Task<IDbContextTransaction> BeginTransactionAsync(bool createSharedTransaction);

    Task<IDbContextTransaction> UseTransactionAsync();
}
