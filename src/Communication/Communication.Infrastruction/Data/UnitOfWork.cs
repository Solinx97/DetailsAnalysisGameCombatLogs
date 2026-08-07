using Communication.Domain.Data;
using Communication.Infrastruction.Persistent;

namespace Communication.Infrastruction.Data;

internal class UnitOfWork(CommunicationContext dbContext) : IUnitOfWork
{
    private readonly CommunicationContext _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
