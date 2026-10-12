using AutoMapper;
using MediatR;
using Notification.Application.DTOs;
using Notification.Domain.Repositories;

namespace Notification.Application.Queries.GetNotificationById;

internal class GetNotificationByIdHandler(INotificationRepository repository, IMapper mapper) : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var notification = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<NotificationDto>(notification);

        return map;
    }
}
