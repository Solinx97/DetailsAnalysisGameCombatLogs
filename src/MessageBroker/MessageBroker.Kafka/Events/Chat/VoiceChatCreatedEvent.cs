using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record VoiceChatCreatedEvent(
    Guid EventId,
    Guid ChatId,
    Guid AppUserId) : IIntegrationEvent;
