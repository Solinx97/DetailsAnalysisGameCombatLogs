using MediatR;

namespace Notification.Application.Commands.CreateNotification;

public record CreateNotificationCommand(
    int Type,
    Guid InitiatorId,
    Guid RecipientId,
    string? InitiatorName
    ) : IRequest<int>;
