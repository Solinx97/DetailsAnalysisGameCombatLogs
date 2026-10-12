using Chat.Application.Consts;
using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.CreateMessage;

internal class CreateMessageHandler(IGroupChatRepository chatRepository, IGroupChatMessageRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateMessageCommand>
{
    private readonly IGroupChatRepository _chatRepository = chatRepository;
    private readonly IGroupChatMessageRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _chatRepository.GetWithUsersAsync(request.ChatId, cancellationToken);

            var message = GroupChatMessage.Create(request.Username, request.Message, request.ChatId, request.GroupChatUserId, chat.LastMessageSequence);
            await _repository.AddAsync(message, cancellationToken);

            chat.IncrementSequence();
            chat.IncreaseUnreadMessages(request.GroupChatUserId);

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
