using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record PersonalChatRemovedEvent(
    Guid EventId,
    int ChatId,
    Guid InitiatorId,
    Guid CompanionId) : IIntegrationEvent;
