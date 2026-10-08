using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Entities.Events;
using Chat.Domain.Repositories;
using MediatR;
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

            var user = chat.Users.First();
            var @event = new GroupChatCreatedEvent(Guid.NewGuid(), chat.Id, user.Id, user.Username, user.AppUserId);
            var outbox = new OutboxMessage
            {
                Id = @event.EventId,
                Topic = KafkaTopics.GROUP_CHAT,
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
