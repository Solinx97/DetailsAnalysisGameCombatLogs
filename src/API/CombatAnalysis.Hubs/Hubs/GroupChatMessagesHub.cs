using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Actions;
using Chat.Application.Security;
using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using CombatAnalysis.Hubs.Patches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

[Authorize]
public class GroupChatMessagesHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatMessagesHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;
    private readonly KafkaSettings _kafkaSettings;

    public GroupChatMessagesHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<GroupChatMessagesHub> logger,
        IKafkaProducerService<string, string> kafkaProducer, IOptions<KafkaSettings> kafkaSettings)
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
        _kafkaSettings = kafkaSettings.Value;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());

            _logger.LogInformation("Clients {Clients} in Group chat message Hub", Clients);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join chat to room failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Join chat to room failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        }
    }

    public async Task SendMessage(GroupChatMessageDto chatMessage)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new GroupChatMessageAction
            {
                InitiatorGroupChatUserId = chatMessage.GroupChatUserId,
                ChatMessage = chatMessage,
                State = ChatMessageActionState.Created,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task RequestMessage(int chatId, GroupChatMessageModel chatMessage)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Requests messages failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RequestEditedMessage(MessagePatch messagePatch)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(messagePatch, nameof(messagePatch));
            ArgumentOutOfRangeException.ThrowIfLessThan(messagePatch.Id, 1, nameof(messagePatch.Id));
            ArgumentOutOfRangeException.ThrowIfLessThan(messagePatch.ChatId, 1, nameof(messagePatch.ChatId));

            await Clients.Group(messagePatch.ChatId.ToString()).SendAsync("ReceiveEditedMessage", messagePatch);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request edited group chat message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task SendMessageHasBeenRead(int chatMessageId, string initiatorGroupChatUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatMessageId, 1, nameof(chatMessageId));
            ArgumentException.ThrowIfNullOrEmpty(initiatorGroupChatUserId, nameof(initiatorGroupChatUserId));

            var response = await _httpClient.GetAsync($"GroupChatMessage/{chatMessageId}");
            response.EnsureSuccessStatusCode();

            var chatMessage = await response.Content.ReadFromJsonAsync<GroupChatMessageDto>();
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new GroupChatMessageAction
            {
                InitiatorGroupChatUserId = initiatorGroupChatUserId,
                ChatMessage = chatMessage,
                State = ChatMessageActionState.Read,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Send message has been read failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task SendMessageRead(int chatId, int chatMessageId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));
            ArgumentOutOfRangeException.ThrowIfZero(chatMessageId, nameof(chatMessageId));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessageHasBeenRead", chatMessageId);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(int room)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(room, 1, nameof(room));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Leave from room failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        }
    }
}
