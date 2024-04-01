using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.CommunicationBL.Interfaces;

public interface ISqlContextService
{
    Task<IDbContextTransaction> BeginTransactionAsync(bool createSharedTransaction);

    Task<IDbContextTransaction> UseTransactionAsync();
}
