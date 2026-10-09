using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Entities.Events;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.AddUser;

internal class AddUserHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IGroupChatMessageRepository messageRepository, IGroupChatUserRepository userRepository,
        IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddUserCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IGroupChatMessageRepository _messageRepository = messageRepository;
    private readonly IGroupChatUserRepository _userRepository = userRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _repository.GetByIdAsync(request.GroupChatId);
            var user = chat.AddUser(request.Username, request.AppUserId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var whoaddUser = await _userRepository.FindChatUserAsync(request.WhoAddAppUserId, request.GroupChatId, cancellationToken);

            var systemMessage = $"'{whoaddUser.Username}' add user '{request.Username}' to chat";
            var message = GroupChatMessage.Create(request.Username, systemMessage, request.GroupChatId, whoaddUser.Id, messageType: MessageType.System);
            await _messageRepository.AddAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, user.GroupChatId, (int)message.Type, whoaddUser.Id, user.Username, systemMessage);
            var outbox = new OutboxMessage
            {
                Id = @event.EventId,
                Topic = KafkaTopics.GROUP_CHAT_MEMBER,
                Key = chat.Id.Value.ToString(),
                Payload = JsonSerializer.Serialize(@event)
            };
            await _boxRepository.AddAsync(outbox, cancellationToken);
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
