using MediatR;

namespace Notification.Application.Commands.DeleteNotification;

public record DeleteNotificationCommand(
    int Id
    ) : IRequest;
