using MediatR;
using Notification.Domain.Repositories;

namespace Notification.Application.Commands.ReadNotification;

internal class ReadNotificationHandler(INotificationRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<ReadNotificationCommand>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _repository.GetByIdAsync(request.Id, cancellationToken);
        notification.Read();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
