using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record VoiceChatCreatedEvent(
    Guid EventId,
    Guid ChatId,
    Guid AppUserId) : IIntegrationEvent;
