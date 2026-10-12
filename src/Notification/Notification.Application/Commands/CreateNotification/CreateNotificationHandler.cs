using MediatR;
using Notification.Application.Consts;
using Notification.Application.Events;
using Notification.Application.Interfaces.Persistence;
using Notification.Domain.Repositories;
using System.Text.Json;

namespace Notification.Application.Commands.CreateNotification;

internal class CreateNotificationHandler(INotificationRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateNotificationCommand>
{
    private readonly INotificationRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var notification = Domain.Aggregates.Notification.Create(request.Type, request.InitiatorId, request.RecipientId,
                    request.InitiatorName);
            await _repository.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new NotificationCreatedEvent(Guid.NewGuid(), notification.Id, notification.InitiatorId, notification.RecipientId, (int)notification.Type, notification.InitiatorName);
            await _boxRepository.AddAsync(@event.EventId, nameof(NotificationCreatedEvent), KafkaTopics.NOTIFICATION, request.RecipientId.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync(transaction, cancellationToken);

            throw;
        }
    }
}
