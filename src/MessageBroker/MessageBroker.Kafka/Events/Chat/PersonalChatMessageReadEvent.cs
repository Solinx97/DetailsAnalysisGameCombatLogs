using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record PersonalChatMessageReadEvent(
    Guid EventId,
    Guid MessageId
    ) : IIntegrationEvent;
