using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

public class PersonalChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatHub> _logger;

    public PersonalChatHub(IHttpClientHelper httpClient, ILogger<PersonalChatHub> logger)
    {
        _httpClient = httpClient;
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

    public async Task CreateChat(string initiatorId, string companionId)
    {
        try
        {
            var personalChat = new PersonalChatModel
            {
                InitiatorId = initiatorId,
                CompanionId = companionId
            };

            var response = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(personalChat));
            response.EnsureSuccessStatusCode();

            var createdChat = await response.Content.ReadFromJsonAsync<PersonalChatModel>();
            if (createdChat == null)
            {
                throw new ArgumentNullException(nameof(createdChat));
            }

            await Clients.Caller.SendAsync("ReceivePersonalChat", createdChat);
            await Clients.Group(companionId).SendAsync("ReceivePersonalChat", createdChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
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
