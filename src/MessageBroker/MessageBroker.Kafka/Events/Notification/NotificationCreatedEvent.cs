using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Notification;

public record NotificationCreatedEvent(
    Guid EventId,
    int NotificationId,
    Guid InitiatorId,
    Guid RecipientId,
    int Type,
    string? InitiatorName) : IIntegrationEvent;
