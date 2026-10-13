using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using System.Text.Json;

namespace Chat.Application.Commands.PersonalChat.UpdateMessageText;

internal class UpdateMessageTextHandler(IGenericRepository<PersonalChatMessage, PersonalChatMessageId> repository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateMessageTextCommand>
{
    private readonly IGenericRepository<PersonalChatMessage, PersonalChatMessageId> _repository = repository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateMessageTextCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(request.Id);
        message.EditMessageText(request.Message);

        var @event = new PersonalChatMessageTextEditedEvent(Guid.NewGuid(), message.Id, message.Message);
        await _boxRepository.AddAsync(@event.EventId, nameof(PersonalChatMessageTextEditedEvent), KafkaTopics.PERSONAL_CHAT_MESSAGE, message.PersonalChatId.Value.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
