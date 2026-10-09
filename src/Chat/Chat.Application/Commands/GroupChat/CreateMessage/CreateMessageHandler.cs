using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.CreateMessage;

internal class CreateMessageHandler(IGroupChatMessageRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageCommand>
{
    private readonly IGroupChatMessageRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var message = GroupChatMessage.Create(request.Username, request.Message, request.ChatId, request.GroupChatUserId);
            await _repository.AddAsync(message, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, message.GroupChatId, (int)message.Type, message.GroupChatUserId, message.Username, message.Message);
            await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, message.GroupChatId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
