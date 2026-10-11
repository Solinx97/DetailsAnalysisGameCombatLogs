using Chat.Application.Interfaces;

namespace Chat.Application.Events;

public record GroupChatMessageReadEvent(
    Guid EventId,
    Guid MessageId
    ) : IIntegrationEvent;
