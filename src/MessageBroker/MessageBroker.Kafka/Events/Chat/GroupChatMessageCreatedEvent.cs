using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    int MessageType,
    Guid SenderId,
    string Username,
    string Message) : IIntegrationEvent;
