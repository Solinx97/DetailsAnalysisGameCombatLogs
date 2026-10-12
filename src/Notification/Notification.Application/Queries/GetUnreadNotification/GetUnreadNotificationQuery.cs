using MediatR;
using Notification.Application.DTOs;

namespace Notification.Application.Queries.GetUnreadNotification;

public record GetUnreadNotificationQuery(
    Guid RecipientId
    ) : IRequest<IEnumerable<NotificationDto>>;
