using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

internal class ChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IHttpClientHelper httpClient, ILogger<ChatHub> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.ChatApi;

        _logger = logger;
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task SendPersonalMessage(string refreshToken, PersonalChatMessageModel message)
    {
        try
        {
            var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), refreshToken);
            response.EnsureSuccessStatusCode();

            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceivePersonalMessage", message.AppUserId, message);
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

    public async Task SendGroupMessage(string refreshToken, PersonalChatMessageModel message)
    {
        try
        {
            var response = await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message), refreshToken);
            response.EnsureSuccessStatusCode();

            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveGroupMessage", message.AppUserId, message);
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
