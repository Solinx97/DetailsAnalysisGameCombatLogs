using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Events;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public class PersonalChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<PersonalChatMessageConsumer> logger, IHubContext<PersonalChatMessagesHub> hubContext) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.PERSONAL_CHAT_MESSAGE, logger)
{
    private readonly ILogger<PersonalChatMessageConsumer> _logger = logger;
    private readonly IHubContext<PersonalChatMessagesHub> _hubContext = hubContext;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            await ExecuteAsync(kafkaData, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Personal Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<PersonalChatMessageCreatedEvent>();
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var message = new PersonalChatMessageDto { Id = @event.MessageId, Message = @event.Message, PersonalChatId = @event.ChatId, AppUserId = @event.SenderId };
            await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceiveMessage", message, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.ParamName);
        }
    }
}
