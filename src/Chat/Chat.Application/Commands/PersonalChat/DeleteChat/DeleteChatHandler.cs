using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.PersonalChat.DeleteChat;

internal class DeleteChatHandler(IGenericRepository<Domain.Aggregates.PersonalChat, PersonalChatId> repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteChatCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.PersonalChat, PersonalChatId> _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _repository.GetByIdAsync(request.Id);
            await _repository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new PersonalChatRemovedEvent(Guid.NewGuid(), chat.Id, chat.InitiatorId, chat.CompanionId);
            await _boxRepository.AddAsync(@event.EventId, nameof(PersonalChatRemovedEvent), KafkaTopics.PERSONAL_CHAT, chat.InitiatorId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
