using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.AddUser;

internal class AddUserHandler(IGroupChatRepository repository, IGroupChatMessageRepository messageRepository, IGroupChatUserRepository userRepository,
        IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IGroupChatMessageRepository _messageRepository = messageRepository;
    private readonly IGroupChatUserRepository _userRepository = userRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _repository.GetWithUsersAsync(request.GroupChatId, cancellationToken);
            var user = chat.AddUser(request.Username, request.AppUserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var whoAddUser = await _userRepository.FindChatUserAsync(request.WhoAddId, request.GroupChatId, cancellationToken);

            var systemMessage = $"'{whoAddUser.Username}' add user '{request.Username}' to chat";
            var message = GroupChatMessage.Create(request.Username, systemMessage, request.GroupChatId, whoAddUser.Id, messageType: MessageType.System);
            await _messageRepository.AddAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var eventSystemMessage = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, chat.Id, (int)message.Type, whoAddUser.Id, user.Username, systemMessage);
            await _boxRepository.AddAsync(eventSystemMessage.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, chat.Id.Value.ToString(), JsonSerializer.Serialize(eventSystemMessage), cancellationToken);

            var @event = new GroupChatCreatedEvent(Guid.NewGuid(), chat.Id, user.Id, user.Username, user.AppUserId);
            await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatCreatedEvent), KafkaTopics.GROUP_CHAT, user.AppUserId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
