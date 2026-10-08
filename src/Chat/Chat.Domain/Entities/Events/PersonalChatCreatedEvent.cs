using Chat.Domain.Interfaces;

namespace Chat.Domain.Entities.Events;

public record PersonalChatCreatedEvent(
    Guid EventId,
    int ChatId,
    Guid InitiatorId,
    Guid CompanionId) : IIntegrationEvent;
