using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Repositories;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.CreateChat;

internal class CreateChatHandler(IGroupChatRepository repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand>
{
    private readonly IGroupChatRepository _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = Domain.Aggregates.GroupChat.Create(request.Name, request.OwnerUsername, request.OwnerId,
                    request.InvitePeopleRule, request.RemovePeopleRule, request.PinMessageRule, request.AnnouncementsRule);
            await _repository.AddAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var groupChatUser = chat.Users.First();
            var @event = new GroupChatCreatedEvent(Guid.NewGuid(), chat.Id, groupChatUser.Id, groupChatUser.Username, groupChatUser.AppUserId);
            await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatCreatedEvent), KafkaTopics.GROUP_CHAT, request.OwnerId.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
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
