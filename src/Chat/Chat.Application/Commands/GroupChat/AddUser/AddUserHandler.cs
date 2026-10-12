using Chat.Application.Consts;
using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.AddUser;

internal class AddUserHandler(IGroupChatRepository repository, IGroupChatMessageRepository messageRepository, IGroupChatUserRepository groupChatUserRepository,
        IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IGroupChatMessageRepository _messageRepository = messageRepository;
    private readonly IGroupChatUserRepository _groupChatUserRepository = groupChatUserRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetWithUsersAsync(request.GroupChatId, cancellationToken);
        var whoAddUser = await _groupChatUserRepository.FindChatUserAsync(request.WhoAddId, request.GroupChatId, cancellationToken);

        var user = chat.AddUser(request.Username, request.AppUserId);

        var systemMessage = $"'{whoAddUser.Username}' add user '{request.Username}' to chat";
        var message = GroupChatMessage.Create(request.Username, systemMessage, request.GroupChatId, whoAddUser.Id, chat.LastMessageSequence, messageType: (int)MessageType.System);
        await _messageRepository.AddAsync(message, cancellationToken);

        var eventSystemMessage = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, chat.Id, (int)message.Type, whoAddUser.Id, user.Username, systemMessage);
        await _boxRepository.AddAsync(eventSystemMessage.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, chat.Id.Value.ToString(), JsonSerializer.Serialize(eventSystemMessage), cancellationToken);

        var @event = new GroupChatCreatedEvent(Guid.NewGuid(), chat.Id, user.Id, user.Username, user.AppUserId);
        await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatCreatedEvent), KafkaTopics.GROUP_CHAT, user.AppUserId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
