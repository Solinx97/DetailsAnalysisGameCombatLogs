using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities.Events;

public record GroupChatCreatedEvent(
    Guid EventId,
    int ChatId,
    Guid GroupChatUserId,
    string Username,
    Guid AppUserId) : IIntegrationEvent;
