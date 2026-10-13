using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatMessageMarkEditedEvent(
    Guid EventId,
    Guid MessageId,
    int NewMark) : IIntegrationEvent;
