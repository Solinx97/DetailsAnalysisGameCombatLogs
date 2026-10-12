using Microsoft.EntityFrameworkCore.Storage;
using Notification.Domain.Repositories;
using Notification.Infrastructure.Persistence;

namespace Notification.Infrastructure.Repositories;

internal class UnitOfWork(NotificationContext dbContext) : IUnitOfWork
{
    private readonly NotificationContext _context = dbContext;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
