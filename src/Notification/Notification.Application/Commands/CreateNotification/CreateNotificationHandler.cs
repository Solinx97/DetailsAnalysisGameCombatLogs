using MediatR;
using Notification.Domain.Repositories;

namespace Notification.Application.Commands.CreateNotification;

internal class CreateNotificationHandler(INotificationRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateNotificationCommand, int>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Domain.Aggregates.Notification.Create(request.Type, request.InitiatorId, request.RecipientId,
                request.InitiatorName);
        await _repository.AddAsync(notification, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return notification.Id;
    }
}
