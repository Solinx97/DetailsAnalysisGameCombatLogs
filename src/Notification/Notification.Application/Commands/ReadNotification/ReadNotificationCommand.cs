using MediatR;

namespace Notification.Application.Commands.ReadNotification;

public record ReadNotificationCommand(
    int Id
    ) : IRequest;
