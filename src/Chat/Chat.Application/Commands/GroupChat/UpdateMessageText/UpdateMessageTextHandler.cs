using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.UpdateMessageText;

internal class UpdateMessageTextHandler(IGenericRepository<GroupChatMessage, GroupChatMessageId> repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateMessageTextCommand>
{
    private readonly IGenericRepository<GroupChatMessage, GroupChatMessageId> _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateMessageTextCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(request.Id);
        message.EditMessageText(request.Message);

        var @event = new GroupChatMessageTextEditedEvent(Guid.NewGuid(), message.Id, message.Message);
        await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatMessageTextEditedEvent), KafkaTopics.GROUP_CHAT_MESSAGE, message.GroupChatId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
