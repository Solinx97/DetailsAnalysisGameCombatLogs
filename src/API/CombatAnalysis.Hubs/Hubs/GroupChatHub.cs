using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using CombatAnalysis.Hubs.Models.Containers;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

public class GroupChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatHub> _logger;

    public GroupChatHub(IHttpClientHelper httpClient, ILogger<GroupChatHub> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;

        _logger = logger;
    }

    public async Task JoinRoom(string appUserId)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, appUserId);
        }
    }

    public async Task CreateGroupChat(GroupChatContainerModel groupChat)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.PostAsync("GroupChat", JsonContent.Create(groupChat), context);
            response.EnsureSuccessStatusCode();

            var createdChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
            if (createdChat == null)
            {
                throw new ArgumentNullException(nameof(createdChat));
            }

            await Clients.Caller.SendAsync("ReceiveGroupChat", createdChat.Id, groupChat.GroupChat.AppUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task RequestJoinedUser(int chatId, string appUserId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.GetAsync($"GroupChatUser/findUserInChat?chatId={chatId}&appUserId={appUserId}", context);
            response.EnsureSuccessStatusCode();

            var groupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            if (groupChatUser == null)
            {
                throw new ArgumentNullException(nameof(groupChatUser));
            }

            await Clients.Group(appUserId).SendAsync("ReceiveJoinedUser", groupChatUser);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task AddUserToChat(GroupChatUserModel groupChatUser)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), context);
            response.EnsureSuccessStatusCode();

            var createdGroupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            if (createdGroupChatUser == null)
            {
                throw new ArgumentNullException(nameof(createdGroupChatUser));
            }

            await Clients.Caller.SendAsync("ReceiveAddedUserToChat", createdGroupChatUser);
            await Clients.Group(groupChatUser.AppUserId).SendAsync("ReceiveAddedUserToChat", createdGroupChatUser);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task LeaveFromRoom(string appUserId)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appUserId);
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
