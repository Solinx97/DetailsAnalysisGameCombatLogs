using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.LeaveFromChat;

internal class LeaveFromChatHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IGroupChatMessageRepository messageRepository, IGroupChatUserRepository userRepository,
        IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<LeaveFromChatCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IGroupChatMessageRepository _messageRepository = messageRepository;
    private readonly IGroupChatUserRepository _userRepository = userRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(LeaveFromChatCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = await _repository.GetByIdAsync(request.GroupChatId);
            var removedUser = chat.RemoveUser(request.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var ownerChatId = await _userRepository.FindChatUserAsync(chat.OwnerId, request.GroupChatId, cancellationToken);

            var systemMessage = $"'{removedUser.Username}' leave from chat";
            var message = GroupChatMessage.Create(removedUser.Username, systemMessage, request.GroupChatId, ownerChatId.Id, messageType: MessageType.System);
            await _messageRepository.AddAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var @event = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, chat.Id, (int)message.Type, ownerChatId.Id, ownerChatId.Username, systemMessage);
            await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatMessageCreatedEvent), KafkaTopics.GROUP_CHAT_MEMBER, chat.Id.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
