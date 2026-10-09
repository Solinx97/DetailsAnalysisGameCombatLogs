namespace Chat.Infrastructure.Persistence.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }

    public string Topic { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }

    public int RetryCount { get; set; }

    public string? Error { get; set; }
}
