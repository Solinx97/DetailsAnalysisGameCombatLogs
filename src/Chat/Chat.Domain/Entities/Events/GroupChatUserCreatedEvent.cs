using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities.Events;

public record GroupChatUserCreatedEvent(
    Guid EventId,
    Guid GroupChatUserId,
    int ChatId,
    string Username) : IIntegrationEvent;
