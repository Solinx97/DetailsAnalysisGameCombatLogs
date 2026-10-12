using MediatR;
using Notification.Application.DTOs;

namespace Notification.Application.Queries.GetNotificationByRecipientId;

public record GetNotificationByRecipientIdQuery(
    Guid RecipientId
    ) : IRequest<IEnumerable<NotificationDto>>;
