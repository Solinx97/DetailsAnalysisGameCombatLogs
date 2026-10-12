using Notification.Application.Interfaces;

namespace Notification.Application.Events;

public record NotificationCreatedEvent(
    Guid EventId,
    int NotificationId,
    Guid InitiatorId,
    Guid RecipientId,
    int Type,
    string? InitiatorName) : IIntegrationEvent;
