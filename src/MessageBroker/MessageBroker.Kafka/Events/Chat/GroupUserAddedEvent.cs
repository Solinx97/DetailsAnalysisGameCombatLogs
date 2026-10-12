using MessageBroker.Kafka.Interfaces;

namespace MessageBroker.Kafka.Events.Chat;

public record GroupUserAddedEvent(
    Guid EventId,
    int ChatId,
    Guid SenderInviteAppUserId,
    string SenderInviteUsername,
    Guid RecipientId,
    string RecipientUsername,
    Guid RecipientAppUserId) : IIntegrationEvent;
