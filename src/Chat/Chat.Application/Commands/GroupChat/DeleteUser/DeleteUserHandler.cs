using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.DeleteUser;

internal class DeleteUserHandler(IGroupChatRepository repository, IGroupChatMessageRepository messageRepository, IGroupChatUserRepository userRepository,
        IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IGroupChatMessageRepository _messageRepository = messageRepository;
    private readonly IGroupChatUserRepository _userRepository = userRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _repository.GetWithUsersAsync(request.GroupChatId, cancellationToken);
            var removedUser = chat.RemoveUser(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var whoDeleteUser = await _userRepository.FindChatUserAsync(request.WhoDeleteId, request.GroupChatId, cancellationToken);

            var systemMessage = $"'{whoDeleteUser.Username}' remove user '{removedUser.Username}' from chat";
            var message = GroupChatMessage.Create(removedUser.Username, systemMessage, request.GroupChatId, whoDeleteUser.Id, messageType: MessageType.System);
            await _messageRepository.AddAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var eventSystemMessage = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, chat.Id, (int)message.Type, whoDeleteUser.Id, removedUser.Username, systemMessage);
            await _boxRepository.AddAsync(eventSystemMessage.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, chat.Id.Value.ToString(), JsonSerializer.Serialize(eventSystemMessage), cancellationToken);

            var @event = new GroupChatUserRemovedEvent(Guid.NewGuid(), chat.Id, removedUser.AppUserId);
            await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatUserRemovedEvent), KafkaTopics.GROUP_CHAT, removedUser.AppUserId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
