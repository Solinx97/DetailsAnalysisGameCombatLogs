using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record PersonalChatMessageMarkEditedEvent(
    Guid EventId,
    Guid MessageId,
    int NewMark) : IIntegrationEvent;
