using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.ChatBL.Interfaces;

public interface ISqlContextService
{
    Task<IDbContextTransaction> BeginTransactionAsync(bool createSharedTransaction);

    Task<IDbContextTransaction> UseTransactionAsync();
}
