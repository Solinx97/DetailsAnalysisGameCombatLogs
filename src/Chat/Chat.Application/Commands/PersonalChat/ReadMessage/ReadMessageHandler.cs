using Chat.Application.Events;
using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Consts;
using Chat.Domain.Repositories;
using MediatR;
using System.Text.Json;

namespace Chat.Application.Commands.PersonalChat.ReadMessage;

internal class ReadMessageHandler(IPersonalChatRepository chatRepository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ReadMessageCommand>
{
    private readonly IPersonalChatRepository _chatRepository = chatRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetWithMessagesAsync(request.ChatId, cancellationToken);
        chat.ReadMessage(request.Id, request.ReaderId);

        var @event = new PersonalChatMessageReadEvent(Guid.NewGuid(), request.Id);
        await _boxRepository.AddAsync(@event.EventId, nameof(PersonalChatMessageReadEvent), KafkaTopics.PERSONAL_CHAT_MESSAGE, request.ChatId.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
