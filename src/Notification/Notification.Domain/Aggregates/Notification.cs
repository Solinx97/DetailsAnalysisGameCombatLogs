using Notification.Domain.Enums;
using Notification.Domain.ValueObjects;

namespace Notification.Domain.Aggregates;

public class Notification
{
    public const int NAME_MAX_LENGTH = 128;

    private Notification() { }

    private Notification(NotificationType type, NotificationStatus status, UserId initiatorId,
        UserId recipientId, DateTimeOffset createdAt, string? initiatorName)
    {
        Type = type;
        Status = status;
        InitiatorId = initiatorId;
        InitiatorName = initiatorName;
        RecipientId = recipientId;
        CreatedAt = createdAt;
    }

    public NotificationId Id { get; private set; }

    public NotificationType Type { get; private set; }

    public NotificationStatus Status { get; private set; }

    public UserId InitiatorId { get; private set; }

    public string? InitiatorName { get; private set; }

    public UserId RecipientId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? ReadAt { get; private set; }

    public static Notification Create(int type, UserId initiatorId,
        UserId recipientId, string? initiatorName)
    {
        var createdAt = DateTimeOffset.UtcNow;
        return new Notification((NotificationType)type, NotificationStatus.Unread, initiatorId, recipientId, createdAt, initiatorName);
    }
}
