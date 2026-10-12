using MediatR;

namespace Notification.Application.Commands.ReadUnreadNotifications;

public record ReadUnreadNotificationsCommand(
    Guid RecipientId
    ) : IRequest;
