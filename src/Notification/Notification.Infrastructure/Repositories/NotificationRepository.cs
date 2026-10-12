using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Domain.ValueObjects;
using Notification.Infrastructure.Exceptions;
using Notification.Infrastructure.Persistence;

namespace Notification.Infrastructure.Repositories;

internal class NotificationRepository(NotificationContext context) : INotificationRepository
{
    private readonly NotificationContext _context = context;

    public async Task AddAsync(Domain.Aggregates.Notification chat, CancellationToken cancelationToken)
    {
        await _context.Notification
                     .AddAsync(chat, cancelationToken);
    }

    public async Task<IEnumerable<Domain.Aggregates.Notification>> GetByRecipientIdAsync(UserId userId, CancellationToken cancelationToken)
    {
        var notifications = await _context.Notification
            .AsNoTracking()
            .Where(x => x.RecipientId == userId)
            .ToListAsync(cancelationToken);

        return notifications;
    }

    public async Task<IEnumerable<Domain.Aggregates.Notification>> GetUnreadAsync(UserId userId, CancellationToken cancelationToken)
    {
        var notifications = await _context.Notification
            .AsNoTracking()
            .Where(x => x.RecipientId == userId && x.Status == Domain.Enums.NotificationStatus.Unread)
            .ToListAsync(cancelationToken);

        return notifications;
    }

    public async Task<Domain.Aggregates.Notification> GetByIdAsync(NotificationId id, CancellationToken cancelationToken)
    {
        var notification = await _context.Notification
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancelationToken)
                ?? throw new EntityNotFoundException(typeof(Domain.Aggregates.Notification), id);

        return notification;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Notification
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
