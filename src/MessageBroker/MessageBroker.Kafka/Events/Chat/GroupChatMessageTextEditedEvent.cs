using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatMessageTextEditedEvent(
    Guid EventId,
    Guid MessageId,
    string NewMessage) : IIntegrationEvent;
