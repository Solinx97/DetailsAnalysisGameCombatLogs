using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class GroupChatMessagesHub(ILogger<GroupChatMessagesHub> logger) : Hub
{
    private readonly ILogger<GroupChatMessagesHub> _logger = logger;

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

    //public async Task RequestEditedMessage(MessagePatch messagePatch)
    //{
    //    try
    //    {
    //        ArgumentNullException.ThrowIfNull(messagePatch, nameof(messagePatch));
    //        ArgumentOutOfRangeException.ThrowIfLessThan(messagePatch.Id, 1, nameof(messagePatch.Id));
    //        ArgumentOutOfRangeException.ThrowIfLessThan(messagePatch.ChatId, 1, nameof(messagePatch.ChatId));

    //        await Clients.Group(messagePatch.ChatId.ToString()).SendAsync("ReceiveEditedMessage", messagePatch);
    //    }
    //    catch (ArgumentOutOfRangeException ex)
    //    {
    //        _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
    //    }
    //    catch (ArgumentNullException ex)
    //    {
    //        _logger.LogError(ex, "Request edited group chat message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
    //    }
    //}

    public async Task SendMessageHasBeenRead(int chatMessageId, string initiatorGroupChatUserId)
    {
        //try
        //{
        //    ArgumentOutOfRangeException.ThrowIfLessThan(chatMessageId, 1, nameof(chatMessageId));
        //    ArgumentException.ThrowIfNullOrEmpty(initiatorGroupChatUserId, nameof(initiatorGroupChatUserId));

        //    var response = await _httpClient.GetAsync($"GroupChatMessage/{chatMessageId}");
        //    response.EnsureSuccessStatusCode();

        //    var chatMessage = await response.Content.ReadFromJsonAsync<GroupChatMessageDto>();
        //    ArgumentNullException.ThrowIfNull(chatMessage, nameof(chatMessage));

        //    var encryptedAccessToken = string.Empty;
        //    var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

        //    if (!string.IsNullOrEmpty(accessToken))
        //    {
        //        encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
        //    }

        //    var chatAction = JsonSerializer.Serialize(new GroupChatMessageAction
        //    {
        //        InitiatorGroupChatUserId = initiatorGroupChatUserId,
        //        ChatMessage = chatMessage,
        //        State = ChatMessageActionState.Read,
        //        When = DateTimeOffset.UtcNow,
        //        AccessToken = encryptedAccessToken
        //    });
        //    await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatAction);
        //}
        //catch (ArgumentOutOfRangeException ex)
        //{
        //    _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        //}
        //catch (ArgumentNullException ex)
        //{
        //    _logger.LogError(ex, "Send message has been read failed. Parameter '{ParamName}' was null.", ex.ParamName);
        //}
        //catch (ArgumentException ex)
        //{
        //    _logger.LogError(ex, "Send message has been read failed. Parameter '{ParamName}' was incorrect.", ex.ParamName);
        //}
        //catch (HttpRequestException ex)
        //{
        //    _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        //}
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
