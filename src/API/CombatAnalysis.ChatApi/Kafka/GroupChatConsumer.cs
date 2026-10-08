using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Domain.Entities.Events;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Hubs;
using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            var @event = JsonDocument.Parse(kafkaData.Message.Value).Deserialize<GroupChatCreatedEvent>();
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var user = new GroupChatUserDto { Id = @event.GroupChatUserId, Username = @event.Username, GroupChatId = @event.ChatId, AppUserId = @event.AppUserId };
            await _hubContext.Clients.Group(@event.AppUserId.ToString()).SendAsync("ReceiveJoinedUser", user, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GROUP_CHAT, ex.ParamName);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed.", KafkaTopics.GROUP_CHAT);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
