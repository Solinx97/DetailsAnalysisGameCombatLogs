using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

internal class GroupChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatHub> _logger;

    public GroupChatHub(IHttpClientHelper httpClient, ILogger<PersonalChatHub> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;

        _logger = logger;
    }

    public async Task JoinRoom(int chatId)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }

    public async Task SendMessage(string message, int chatId, string groupChatUserId, string username)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var groupMessage = new GroupChatMessageModel
            {
                Username = username,
                Message = message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
                Status = 0,
                ChatId = chatId,
                GroupChatUserId = groupChatUserId
            };

            var response = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(groupMessage), context);
            response.EnsureSuccessStatusCode();

            var createdMessage = await response.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            if (createdMessage == null)
            {
                throw new ArgumentNullException(nameof(createdMessage));
            }

            await Clients.Caller.SendAsync("MessageDelivered");

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", createdMessage);
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

    public async Task SendMessageHasBeenRead(int chatMessageId, string meInChatId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = await _httpClient.GetAsync($"GroupChatMessage/{chatMessageId}", context);
            response.EnsureSuccessStatusCode();

            var messageModel = await response.Content.ReadFromJsonAsync<GroupChatMessageModel>();
            if (messageModel == null)
            {
                throw new ArgumentNullException(nameof(messageModel));
            }

            if (messageModel.Status == 2)
            {
                return;
            }

            messageModel.Status = 2;

            response = await _httpClient.PutAsync("GroupChatMessage", JsonContent.Create(messageModel), context);
            response.EnsureSuccessStatusCode();

            response = await _httpClient.GetAsync($"GroupChatMessageCount/findMe?chatId={messageModel.ChatId}&chatUserId={meInChatId}", context);
            response.EnsureSuccessStatusCode();

            var messageCount = await response.Content.ReadFromJsonAsync<GroupChatMessageCountModel>();
            if (messageCount == null)
            {
                throw new ArgumentNullException(nameof(messageCount));
            }

            messageCount.Count--;

            response = await _httpClient.PutAsync("GroupChatMessageCount", JsonContent.Create(messageCount), context);
            response.EnsureSuccessStatusCode();

            await Clients.Caller.SendAsync("ReceiveMessageHasBeenRead");
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
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        }
    }
}
