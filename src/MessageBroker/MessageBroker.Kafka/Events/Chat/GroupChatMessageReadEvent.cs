using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatMessageReadEvent(
    Guid EventId,
    Guid MessageId
    ) : IIntegrationEvent;
