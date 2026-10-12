using CombatAnalysis.NotificationAPI.Enums;
using CombatAnalysis.NotificationAPI.Hubs;
using Confluent.Kafka;
using MediatR;
using MessageBroker.Kafka.Consts;
using MessageBroker.Kafka.Events.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Notification.Application.Commands.CreateNotification;
using Notification.Application.DTOs;
using System.Text;
using System.Text.Json;

namespace CombatAnalysis.NotificationAPI.Kafka;

public class NotificationConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<NotificationConsumer> logger, IHubContext<NotificationHub> hubContext, IServiceScopeFactory scopeFactory)
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.GROUP_CHAT, logger)
{
    private readonly ILogger<NotificationConsumer> _logger = logger;
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

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
            await using var scope = _scopeFactory.CreateAsyncScope();

            switch (eventType)
            {
                case nameof(GroupUserAddedEvent):
                    var eventAdd = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupUserAddedEvent>();
                    ArgumentNullException.ThrowIfNull(eventAdd, nameof(eventAdd));

                    var mediator = scope.ServiceProvider
                        .GetRequiredService<IMediator>();

                    var notificationType = NotificationType.AddToGroupChat;
                    var command = new CreateNotificationCommand((int)notificationType, eventAdd.SenderInviteAppUserId,
                                eventAdd.RecipientAppUserId, eventAdd.SenderInviteUsername);
                    var notificationId = await mediator.Send(command, cancellationToken);

                    var notification = new NotificationDto { Id = notificationId, Type = (int)notificationType, InitiatorId = eventAdd.SenderInviteAppUserId, RecipientId = eventAdd.RecipientAppUserId, InitiatorName = eventAdd.SenderInviteUsername };
                    await _hubContext.Clients.User(kafkaData.Message.Key).SendAsync("ReceiveNotification", notification, cancellationToken);
                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT, ex.ParamName);
        }
    }
}
