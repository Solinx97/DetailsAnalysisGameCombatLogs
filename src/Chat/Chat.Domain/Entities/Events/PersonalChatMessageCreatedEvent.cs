using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities.Events;

public record PersonalChatMessageCreatedEvent(
    Guid EventId,
    Guid MessageId,
    int ChatId,
    Guid SenderId,
    string Message) : IIntegrationEvent;
