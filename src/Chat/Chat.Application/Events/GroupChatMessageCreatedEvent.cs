using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record GroupChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    int MessageType,
    Guid SenderId,
    string Username,
    string Message) : IIntegrationEvent;
