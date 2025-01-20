using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

internal class PersonalChatUnreadMessageHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatUnreadMessageHub> _logger;

    public PersonalChatUnreadMessageHub(IHttpClientHelper httpClient, ILogger<PersonalChatUnreadMessageHub> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;

        _logger = logger;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task SendUnreadMessageUpdated(int chatId)
    {
        try
        {
            await Clients.OthersInGroup(chatId.ToString()).SendAsync("ReceiveUnreadMessageUpdated", chatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task RequestUnreadMessages(int chatId, string appUserId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.GetAsync($"PersonalChatMessageCount/findMe?chatId={chatId}&appUserId={appUserId}", context);
            response.EnsureSuccessStatusCode();

            var messagesCount = await response.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            if (messagesCount == null)
            {
                throw new ArgumentNullException(nameof(messagesCount));
            }

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveUnreadMessage", chatId, appUserId, messagesCount.Count);
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

    public async Task LeaveFromRoom(int room)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
    }
}
