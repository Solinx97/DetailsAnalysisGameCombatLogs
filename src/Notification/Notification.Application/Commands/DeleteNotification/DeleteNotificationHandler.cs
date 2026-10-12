using MediatR;
using Notification.Domain.Repositories;

namespace Notification.Application.Commands.DeleteNotification;

internal class DeleteNotificationHandler(INotificationRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteNotificationCommand>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
