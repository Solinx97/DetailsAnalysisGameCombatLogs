using MediatR;
using Notification.Domain.Repositories;

namespace Notification.Application.Commands.ReadUnreadNotifications;

internal class ReadUnreadNotificationsHandler(INotificationRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<ReadUnreadNotificationsCommand>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(ReadUnreadNotificationsCommand request, CancellationToken cancellationToken)
    {
        await _repository.ReadAllUnreadAsync(request.RecipientId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

