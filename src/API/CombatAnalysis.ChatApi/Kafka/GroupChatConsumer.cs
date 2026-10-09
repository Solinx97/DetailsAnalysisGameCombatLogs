using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Events;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public class GroupChatConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatConsumer> logger, IHubContext<GroupChatHub> hubContext) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.GROUP_CHAT, logger)
{
    private readonly ILogger<GroupChatConsumer> _logger = logger;
    private readonly IHubContext<GroupChatHub> _hubContext = hubContext;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            await ExecuteAsync(kafkaData, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Group Chat data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            var eventTypeHeader = kafkaData.Message.Headers?.LastOrDefault(x => x.Key == "event-type")
                ?? throw new InvalidOperationException("Missing event-type header.");

            var eventType = Encoding.UTF8.GetString(eventTypeHeader.GetValueBytes());
            switch (eventType)
            {
                case nameof(GroupChatCreatedEvent):
                    var eventCreate = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatCreatedEvent>();
                    ArgumentNullException.ThrowIfNull(eventCreate, nameof(eventCreate));

                    var owner = new GroupChatUserDto { Id = eventCreate.GroupChatUserId, Username = eventCreate.Username, GroupChatId = eventCreate.ChatId, AppUserId = eventCreate.AppUserId };
                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceiveJoinedUser", owner, cancellationToken);
                    break;
                case nameof(GroupChatUserRemovedEvent):
                    var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatUserRemovedEvent>();
                    ArgumentNullException.ThrowIfNull(@event, nameof(@event));

                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("UserRemoved", @event.AppUserId.ToString(), @event.ChatId, cancellationToken);
                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT, ex.ParamName);
        }
    }
}
