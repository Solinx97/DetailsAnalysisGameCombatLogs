using MediatR;
using Notification.Application.DTOs;

namespace Notification.Application.Queries.GetNotificationById;

public record GetNotificationByIdQuery(
    int Id
    ) : IRequest<NotificationDto>;
