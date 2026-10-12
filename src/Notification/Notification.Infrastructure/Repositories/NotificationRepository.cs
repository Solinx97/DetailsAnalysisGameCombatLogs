using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Domain.ValueObjects;
using Notification.Infrastructure.Exceptions;
using Notification.Infrastructure.Persistence;

namespace Notification.Infrastructure.Repositories;

internal class NotificationRepository(NotificationContext context) : INotificationRepository
{
    private readonly NotificationContext _context = context;

    public async Task AddAsync(Domain.Aggregates.Notification chat, CancellationToken cancellationToken)
    {
        await _context.Notification
                     .AddAsync(chat, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Aggregates.Notification>> GetByRecipientIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notification
            .AsNoTracking()
            .Where(x => x.RecipientId == userId)
            .ToListAsync(cancellationToken);

        return notifications;
    }

    public async Task<IEnumerable<Domain.Aggregates.Notification>> GetUnreadAsync(UserId userId, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notification
            .AsNoTracking()
            .Where(x => x.RecipientId == userId && x.Status == Domain.Enums.NotificationStatus.Unread)
            .ToListAsync(cancellationToken);

        return notifications;
    }

    public async Task<Domain.Aggregates.Notification> GetByIdAsync(NotificationId id, CancellationToken cancellationToken)
    {
        var notification = await _context.Notification
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Domain.Aggregates.Notification), id);

        return notification;
    }

    public async Task ReadAllUnreadAsync(UserId recipientId, CancellationToken cancellationToken)
    {
        await _context.Notification
            .Where(n => n.RecipientId == recipientId && n.ReadAt == null)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(
                    n => n.ReadAt,
                    DateTime.UtcNow)
                .SetProperty(
                    n => n.Status,
                    Domain.Enums.NotificationStatus.Read),
                cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _context.Notification
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
