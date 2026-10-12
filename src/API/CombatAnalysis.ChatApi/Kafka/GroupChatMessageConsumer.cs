using Chat.Application.DTOs;
using Chat.Domain.Enums;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageConsumer> logger, IHubContext<GroupChatMessagesHub> hubContext) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.GROUP_CHAT_MESSAGE, logger)
{
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
    private readonly IHubContext<GroupChatMessagesHub> _hubContext = hubContext;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            await ExecuteActionAsync(kafkaData, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Group Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT_MESSAGE, ex.ParamName);
        }
    }

    private async Task ExecuteActionAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            var eventTypeHeader = kafkaData.Message.Headers?.LastOrDefault(x => x.Key == "event-type")
                ?? throw new InvalidOperationException("Missing event-type header.");

            var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
            switch (eventType)
            {
                case nameof(GroupChatMessageCreatedEvent):
                    var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatMessageCreatedEvent>();
                    ArgumentNullException.ThrowIfNull(@event, nameof(@event));

                    var message = new GroupChatMessageDto { Id = @event.MessageId, Status = MessageStatus.Sent, Type = (MessageType)@event.MessageType, Username = @event.Username, Message = @event.Message, GroupChatId = @event.ChatId, GroupChatUserId = @event.SenderId };
                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceiveMessage", message, cancellationToken);
                    break;
                case nameof(GroupChatMessageReadEvent):
                    var eventRead = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatMessageReadEvent>();
                    ArgumentNullException.ThrowIfNull(eventRead, nameof(eventRead));

                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceiveReadMessage", eventRead.MessageId, cancellationToken);
                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT_MESSAGE, ex.ParamName);
        }
    }
}
