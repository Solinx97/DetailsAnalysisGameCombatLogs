using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Domain.Entities.Events;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<PersonalChatCreatedEvent>();
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var chat = new PersonalChatDto { Id = @event.ChatId, InitiatorId = @event.InitiatorId, CompanionId = @event.CompanionId };
            await _hubContext.Clients.Group(chat.InitiatorId.ToString()).SendAsync("ReceivePersonalChat", chat, cancellationToken);
            await _hubContext.Clients.Group(kafkaData.Message.Key).SendAsync("ReceivePersonalChat", chat, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create personal chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.ParamName);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat {Id} not found.", KafkaTopics.PERSONAL_CHAT_MESSAGE, ex.EntityId);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Update personal chat from Kafka Consumer (topic: {Topic}) failed. Personal chat not found or modified.", KafkaTopics.PERSONAL_CHAT_MESSAGE);
        }
    }
}
