using AutoMapper;
using MediatR;
using Notification.Application.DTOs;
using Notification.Domain.Repositories;

namespace Notification.Application.Queries.GetNotificationByRecipientId;

internal class GetNotificationByRecipientIdHandler(INotificationRepository repository, IMapper mapper) : IRequestHandler<GetNotificationByRecipientIdQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationByRecipientIdQuery request, CancellationToken cancellationToken)
    {
        var notifications = await _repository.GetByRecipientIdAsync(request.RecipientId, cancellationToken);
        var map = _mapper.Map<IEnumerable<NotificationDto>>(notifications);

        return map;
    }
}
