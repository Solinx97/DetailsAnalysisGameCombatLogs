namespace Notification.Application.DTOs;

public class NotificationDto
{
    public int Id { get; set; }

    public int Type { get; set; }

    public int Status { get; set; }

    public Guid InitiatorId { get; set; }

    public string? InitiatorName { get; set; }

    public Guid RecipientId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ReadAt { get; set; }
}
