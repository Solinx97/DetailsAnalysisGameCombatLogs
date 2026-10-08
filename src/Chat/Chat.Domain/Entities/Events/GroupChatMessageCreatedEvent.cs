using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities.Events;

public record GroupChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    Guid SenderId,
    string Username,
    string Message) : IIntegrationEvent;
