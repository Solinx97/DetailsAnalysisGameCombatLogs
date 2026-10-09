using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.PersonalChat.CreateChat;

internal class CreateChatHandler(IPersonalChatRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand>
{
    private readonly IPersonalChatRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = Domain.Aggregates.PersonalChat.Create(request.InitiatorId, request.CompanionId);
            await _repository.AddAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new PersonalChatCreatedEvent(Guid.NewGuid(), chat.Id, chat.InitiatorId, chat.CompanionId);
            await _boxRepository.AddAsync(@event.EventId, nameof(PersonalChatCreatedEvent), KafkaTopics.PERSONAL_CHAT, chat.InitiatorId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
