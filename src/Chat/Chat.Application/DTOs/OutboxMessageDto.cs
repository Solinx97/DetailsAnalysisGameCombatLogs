namespace Chat.Application.DTOs;

public record OutboxMessageDto(
    Guid Id,
    string Topic,
    string Key,
    string EventType,
    string Payload,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ProcessedAt,
    int RetryCount,
    string? Error
    );
