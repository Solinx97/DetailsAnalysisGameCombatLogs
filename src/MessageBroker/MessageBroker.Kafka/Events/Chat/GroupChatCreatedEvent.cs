using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatCreatedEvent(
    Guid EventId,
    int ChatId,
    Guid GroupChatUserId,
    string Username,
    Guid AppUserId) : IIntegrationEvent;
