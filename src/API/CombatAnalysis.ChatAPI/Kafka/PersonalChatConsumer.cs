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

public class PersonalChatConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<PersonalChatConsumer> logger, IHubContext<PersonalChatHub> hubContext)
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.PERSONAL_CHAT, logger)
{
    private readonly ILogger<PersonalChatConsumer> _logger = logger;
    private readonly IHubContext<PersonalChatHub> _hubContext = hubContext;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            await ExecuteAsync(kafkaData, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Personal Chat data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.ParamName);
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
                case nameof(PersonalChatCreatedEvent):
                    var eventCreate = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<PersonalChatCreatedEvent>();
                    ArgumentNullException.ThrowIfNull(eventCreate, nameof(eventCreate));

                    var chat = new PersonalChatDto { Id = eventCreate.ChatId, InitiatorId = eventCreate.InitiatorId, CompanionId = eventCreate.CompanionId };
                    await _hubContext.Clients.Group(chat.CompanionId.ToString()).SendAsync("ReceivePersonalChat", chat, cancellationToken);
                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceivePersonalChat", chat, cancellationToken);
                    break;
                case nameof(PersonalChatRemovedEvent):
                    var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<PersonalChatRemovedEvent>();
                    ArgumentNullException.ThrowIfNull(@event, nameof(@event));

                    await _hubContext.Clients.Group(@event.CompanionId.ToString()).SendAsync("UserRemoved", @event.CompanionId.ToString(), @event.ChatId, cancellationToken);
                    await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("UserRemoved", @event.InitiatorId.ToString(), @event.ChatId, cancellationToken);
                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.ParamName);
        }
    }
}
