using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record GroupChatCreatedEvent(
    Guid EventId,
    int ChatId,
    Guid GroupChatUserId,
    string Username,
    Guid AppUserId) : IIntegrationEvent;
