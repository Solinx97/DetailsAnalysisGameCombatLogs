using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record PersonalChatCreatedEvent(
    Guid EventId,
    int ChatId,
    Guid InitiatorId,
    Guid CompanionId) : IIntegrationEvent;
