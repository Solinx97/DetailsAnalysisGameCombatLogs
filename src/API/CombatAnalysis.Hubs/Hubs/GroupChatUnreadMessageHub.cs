using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

internal class GroupChatUnreadMessageHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatUnreadMessageHub> _logger;

    public GroupChatUnreadMessageHub(IHttpClientHelper httpClient, ILogger<GroupChatUnreadMessageHub> logger)
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

    public async Task SendUndreadMessageCount(int chatId, string appUserId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.GetAsync($"GroupChatMessageCount/find?chatId={chatId}&appUserId={appUserId}", context);
            response.EnsureSuccessStatusCode();

            var messagesCount = await response.Content.ReadFromJsonAsync<GroupChatMessageCountModel>();
            if (messagesCount == null)
            {
                throw new ArgumentNullException(nameof(messagesCount));
            }

            await Clients.OthersInGroup(chatId.ToString()).SendAsync("ReceiveUnreadMessageCount", chatId, messagesCount.Count);
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
}
