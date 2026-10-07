using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Actions;
using Chat.Application.Security;
using Chat.Domain.Enums;
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
public class PersonalChatMessagesHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatMessagesHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;
    private readonly KafkaSettings _kafkaSettings;

    public PersonalChatMessagesHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<PersonalChatMessagesHub> logger,
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
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                throw new HubException("User is not authenticated.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Join chat to room failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        }
    }

    public async Task SendMessage(PersonalChatMessageDto chatMessage, string recipientId)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));
            ArgumentException.ThrowIfNullOrEmpty(recipientId, nameof(recipientId));

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new PersonalChatMessageAction
            {
                RecipientId = recipientId,
                ChatMessage = chatMessage,
                State = ChatMessageActionState.Created,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request unread messages failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Request unread messages failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task RequestMessage(int chatId, PersonalChatMessageModel chatMessage)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Requests messages failed: Parameter '{ParamName}' was null.", ex.ParamName);
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
            _logger.LogError(ex, "Request edited personal chat message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task SendMessageHasBeenRead(int chatMessageId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatMessageId, 1, nameof(chatMessageId));

            var response = await _httpClient.GetAsync($"PersonalChatMessage/{chatMessageId}");
            response.EnsureSuccessStatusCode();

            var chatMessage = await response.Content.ReadFromJsonAsync<PersonalChatMessageDto>();
            ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

            if (chatMessage.Status == MessageStatus.Read)
            {
                return;
            }

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new PersonalChatMessageAction
            {
                ChatMessage = chatMessage,
                State = ChatMessageActionState.Read,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.PersonalChatMessage, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Send message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied. User should be authorized.");
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
