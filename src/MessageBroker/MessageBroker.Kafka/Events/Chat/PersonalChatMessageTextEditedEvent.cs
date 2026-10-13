using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record PersonalChatMessageTextEditedEvent(
    Guid EventId,
    Guid MessageId,
    string NewMessage) : IIntegrationEvent;
