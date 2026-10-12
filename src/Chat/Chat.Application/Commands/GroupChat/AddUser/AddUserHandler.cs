using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
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
        var senderInvite = await _groupChatUserRepository.FindChatUserAsync(request.WhoAddId, request.GroupChatId, cancellationToken);

        var recipient = chat.AddUser(request.Username, request.AppUserId);

        var systemMessage = $"'{senderInvite.Username}' add user '{request.Username}' to chat";
        var messageId = Guid.NewGuid();
        var message = GroupChatMessage.Create(messageId, request.Username, systemMessage, request.GroupChatId, senderInvite.Id, chat.LastMessageSequence, messageType: (int)MessageType.System);
        await _messageRepository.AddAsync(message, cancellationToken);

        var eventSystemMessage = new GroupChatMessageCreatedEvent(Guid.NewGuid(), messageId, chat.Id, (int)message.Type, senderInvite.Id, recipient.Username, systemMessage);
        await _boxRepository.AddAsync(eventSystemMessage.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, chat.Id.Value.ToString(), JsonSerializer.Serialize(eventSystemMessage), cancellationToken);

        var @event = new GroupUserAddedEvent(Guid.NewGuid(), chat.Id, senderInvite.AppUserId, senderInvite.Username, recipient.Id, recipient.Username, recipient.AppUserId);
        await _boxRepository.AddAsync(@event.EventId, nameof(GroupUserAddedEvent), KafkaTopics.GROUP_CHAT, recipient.AppUserId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
