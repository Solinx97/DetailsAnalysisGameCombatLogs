using Chat.Application.Interfaces.Persistence;
using Chat.Domain.Repositories;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using System.Text.Json;

namespace Chat.Application.Commands.GroupChat.ReadMessage;

internal class ReadMessageHandler(IGroupChatRepository chatRepository, IOutboxRepository boxRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ReadMessageCommand>
{
    private readonly IGroupChatRepository _chatRepository = chatRepository;
    private readonly IOutboxRepository _boxRepository = boxRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(ReadMessageCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetWithAllAsync(request.ChatId, cancellationToken);
        chat.ReadMessage(request.Id, request.ReaderId);

        var @event = new GroupChatMessageReadEvent(Guid.NewGuid(), request.Id);
        await _boxRepository.AddAsync(@event.EventId, nameof(GroupChatMessageReadEvent), KafkaTopics.GROUP_CHAT_MESSAGE, request.ChatId.ToString(), JsonSerializer.Serialize(@event), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
