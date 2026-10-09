using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record PersonalChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    Guid SenderId,
    string Message) : IIntegrationEvent;
