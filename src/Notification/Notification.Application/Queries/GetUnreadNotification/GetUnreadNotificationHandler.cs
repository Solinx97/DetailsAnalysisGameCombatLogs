using AutoMapper;
using MediatR;
using Notification.Application.DTOs;
using Notification.Domain.Repositories;

namespace Notification.Application.Queries.GetUnreadNotification;

internal class GetUnreadNotificationHandler(INotificationRepository repository, IMapper mapper) : IRequestHandler<GetUnreadNotificationQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<NotificationDto>> Handle(GetUnreadNotificationQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.GetUnreadAsync(request.RecipientId, cancellationToken);
        var map = _mapper.Map<IEnumerable<NotificationDto>>(notifications);

        return map;
    }
}
