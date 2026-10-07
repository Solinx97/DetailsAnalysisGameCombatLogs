using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.Hubs.Hubs;

[Authorize]
public class GroupChatUnreadMessageHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatUnreadMessageHub> _logger;

    public GroupChatUnreadMessageHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<GroupChatUnreadMessageHub> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join chat to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task SendUnreadMessageUpdated(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            await Clients.OthersInGroup(chatId.ToString()).SendAsync("ReceiveUnreadMessageUpdated", chatId);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    public async Task RequestUnreadMessages(int chatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"GroupChatUser/findAll/{chatId}");
            response.EnsureSuccessStatusCode();

            var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            foreach (var user in groupChatUsers)
            {
                await Clients.Group(chatId.ToString()).SendAsync("ReceiveUnreadMessage", chatId, user.Id, user.UnreadMessages);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request unread messages failed: Parameter '{ParamName}' was null.", ex.ParamName);
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

    public async Task LeaveFromRoom(int room)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(room, 1, nameof(room));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Leave from room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }
}
