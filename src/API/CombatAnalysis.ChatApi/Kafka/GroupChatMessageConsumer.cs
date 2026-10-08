using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Domain.Exceptions;
using Chat.Infrastructure.Outbox.Events;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMessageConsumer> logger, IHubContext<GroupChatMessagesHub> hubContext) 
    : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
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
            _logger.LogError(ex, "Consume Group Chat Message data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
    }

    private async Task ExecuteActionAsync(ConsumeResult<string, string> kafkaData, CancellationToken cancellationToken)
    {
        try
        {
            var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatMessageCreatedEvent>();
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var message = new GroupChatMessageDto { Id = @event.MessageId, Message = @event.Message, GroupChatId = @event.ChatId, GroupChatUserId = @event.SenderId, AppUserId = @event.SenderId };
            await _hubContext.Clients.Group(message.GroupChatId.ToString()).SendAsync("ReceiveMessage", message, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat message from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMessage, ex.ParamName);
        }
        catch (GroupChatNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message from Kafka Consumer (topic: {Topic}) failed. Group chat {Id} not found.", KafkaTopics.GroupChatMessage, ex.GroupChatId);
        }
        catch (GroupChatUserNotFoundException ex)
        {
            _logger.LogWarning("Create group chat message from Kafka Consumer (topic: {Topic}) failed. Group chat user {Id} not found.", KafkaTopics.GroupChatMessage, ex.UserId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create group chat message from Kafka Consumer (topic: {Topic}).", KafkaTopics.GroupChatMessage);
        }
    }
}
