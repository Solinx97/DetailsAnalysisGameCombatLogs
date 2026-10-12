using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatUserRemovedEvent(
    Guid EventId,
    int ChatId,
    Guid AppUserId) : IIntegrationEvent;
