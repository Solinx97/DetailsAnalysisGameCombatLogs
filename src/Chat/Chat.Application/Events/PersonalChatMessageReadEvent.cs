using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record PersonalChatMessageReadEvent(
    Guid EventId,
    Guid MessageId
    ) : IIntegrationEvent;
