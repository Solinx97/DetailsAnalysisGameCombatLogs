using CombatAnalysis.NotificationAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Notification.Application.Consts;
using Notification.Application.DTOs;
using Notification.Application.Events;
using System.Text;
using System.Text.Json;

namespace CombatAnalysis.NotificationAPI.Kafka;

public class NotificationConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<NotificationConsumer> logger, IHubContext<NotificationHub> hubContext)
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.NOTIFICATION, logger)
{
    private readonly ILogger<NotificationConsumer> _logger = logger;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            await ExecuteAsync(kafkaData, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Group Chat data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.NOTIFICATION, ex.ParamName);
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
                case nameof(NotificationCreatedEvent):
                    var eventCreate = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<NotificationCreatedEvent>();
                    ArgumentNullException.ThrowIfNull(eventCreate, nameof(eventCreate));

                    var notification = new NotificationDto { Id = eventCreate.NotificationId, Type = eventCreate.Type, InitiatorId = eventCreate.InitiatorId, RecipientId = eventCreate.RecipientId, InitiatorName = eventCreate.InitiatorName };
                    await _hubContext.Clients.User(kafkaData.Message.Key).SendAsync("ReceiveNotification", notification, cancellationToken);
                    break;
                //case nameof(GroupChatUserRemovedEvent):
                //    var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatUserRemovedEvent>();
                //    ArgumentNullException.ThrowIfNull(@event, nameof(@event));

                //    await _hubContext.Clients.User(kafkaData.Message.Key).SendAsync("UserRemoved", @event.AppUserId.ToString(), @event.ChatId, cancellationToken);
                //    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.NOTIFICATION, ex.ParamName);
        }
    }
}
