using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record PersonalChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    Guid SenderId,
    string Message) : IIntegrationEvent;
