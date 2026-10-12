using Notification.Domain.ValueObjects;

namespace Notification.Domain.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Aggregates.Notification chat, CancellationToken cancelationToken);

    Task<IEnumerable<Aggregates.Notification>> GetByRecipientIdAsync(UserId userId, CancellationToken cancelationToken);

    Task<IEnumerable<Aggregates.Notification>> GetUnreadAsync(UserId userId, CancellationToken cancelationToken);

    Task<Aggregates.Notification> GetByIdAsync(NotificationId id, CancellationToken cancelationToken);

    Task ReadAllUnreadAsync(UserId recipientId, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
