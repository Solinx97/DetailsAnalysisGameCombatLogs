using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

internal class PersonalChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatHub> _logger;

    public PersonalChatHub(IHttpClientHelper httpClient, ILogger<PersonalChatHub> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;

        _logger = logger;
    }

    public async Task JoinRoom(string room)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies["RefreshToken"] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }
    }

    public async Task SendMessage(string message, int chatId, string appUserId, string username)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var personalMessage = new PersonalChatMessageModel
            {
                Username = username,
                Message = message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
                Status = 0,
                ChatId = chatId,
                AppUserId = appUserId
            };

            var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(personalMessage), context);
            response.EnsureSuccessStatusCode();

            await SendUndreadMessageCount(chatId, appUserId);

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", personalMessage);
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

    public async Task SendUndreadMessageCount(int chatId, string appUserId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.GetAsync($"PersonalChatMessageCount/find?chatId={chatId}&appUserId={appUserId}", context);
            response.EnsureSuccessStatusCode();

            var messagesCount = await response.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            if (messagesCount == null)
            {
                throw new ArgumentNullException(nameof(messagesCount));
            }

            messagesCount.Count++;

            response = await _httpClient.PutAsync("PersonalChatMessageCount", JsonContent.Create(messagesCount), context);
            response.EnsureSuccessStatusCode();

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveUnreadMessageCount", messagesCount);
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

    public async Task LeaveFromRoom(string room)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies["RefreshToken"] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        }
    }
}
