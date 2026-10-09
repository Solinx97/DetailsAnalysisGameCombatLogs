using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class GroupChatHub(ILogger<GroupChatHub> logger) : Hub
{
    private readonly ILogger<GroupChatHub> _logger = logger;

    public async Task JoinRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                throw new HubException("User is not authenticated.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RequestMembers(int chatId, string appUserId)
    {
        try
        {
            //ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            //ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            //var response = await _httpClient.GetAsync($"GroupChatUser/findAll/{chatId}");
            //response.EnsureSuccessStatusCode();

            //var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            //ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            //await Clients.Group(appUserId).SendAsync("ReceiveMembers", groupChatUsers);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request joined users failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task RemoveUserFromChat(string chatOwnerId, int chatId, string groupChatUserId, string groupChatUsername)
    {
        try
        {
            //ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            //ArgumentNullException.ThrowIfNullOrEmpty(groupChatUserId, nameof(groupChatUserId));
            //ArgumentNullException.ThrowIfNullOrEmpty(groupChatUsername, nameof(groupChatUsername));

            //var chatAction = JsonSerializer.Serialize(new GroupChatMemberAction
            //{
            //    ChatOwnerId = chatOwnerId,
            //    User = new GroupChatUserDto
            //    {
            //        Id = groupChatUserId,
            //        Username = groupChatUsername,
            //        GroupChatId = chatId,
            //    },
            //    State = ChatMembersActionState.RemoveUser,
            //    When = DateTime.UtcNow,
            //    AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            //});
            //await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMember, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Add user to chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogError(exception, exception.Message);
        }

        return base.OnDisconnectedAsync(exception);
    }
}
