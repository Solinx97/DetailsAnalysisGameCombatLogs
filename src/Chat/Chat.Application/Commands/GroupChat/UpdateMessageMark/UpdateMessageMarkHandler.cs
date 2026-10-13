using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.UpdateMessageMark;

internal class UpdateMessageMarkHandler(IGenericRepository<GroupChatMessage, GroupChatMessageId> repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateMessageMarkCommand>
{
    private readonly IGenericRepository<GroupChatMessage, GroupChatMessageId> _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateMessageMarkCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(request.Id);
        message.EditMark(request.Mark);

        var @event = new GroupChatMessageMarkEditedEvent(Guid.NewGuid(), message.Id, (int)message.MarkedType);
        await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatMessageMarkEditedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, message.GroupChatId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
