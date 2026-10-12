namespace Notification.Domain.ValueObjects;

public record NotificationId
{
    public NotificationId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(NotificationId id) => id.Value;

    public static implicit operator NotificationId(int value) => new(value);
}
