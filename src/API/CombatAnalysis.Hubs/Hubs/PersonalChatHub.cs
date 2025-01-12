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
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task SendMessage(string refreshToken, PersonalChatMessageModel message)
    {
        try
        {
            var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), refreshToken);
            response.EnsureSuccessStatusCode();

            await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
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
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
    }
}
