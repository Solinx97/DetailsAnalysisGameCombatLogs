using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record GroupChatUserRemovedEvent(
    Guid EventId,
    int ChatId,
    Guid AppUserId) : IIntegrationEvent;
