using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.PersonalChat.CreateMessage;

internal class CreateMessageHandler(IPersonalChatMessageRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageCommand>
{
    private readonly IPersonalChatMessageRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var message = PersonalChatMessage.Create(request.Username, request.Message, request.ChatId, request.AppUserId);
            await _repository.AddAsync(message, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new PersonalChatMessageCreatedEvent(Guid.NewGuid(), message.Id, message.PersonalChatId, message.AppUserId, message.Message);
            await _boxRepository.AddAsync(@event.EventId, nameof(PersonalChatMessageCreatedEvent), KafkaTopics.PERSONAL_CHAT_MESSAGE, message.PersonalChatId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
