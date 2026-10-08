using Chat.Infrastructure.Interfaces;

namespace Chat.Infrastructure.Outbox.Events;

public record GroupChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    string SenderId,
    string Message) : IIntegrationEvent;
