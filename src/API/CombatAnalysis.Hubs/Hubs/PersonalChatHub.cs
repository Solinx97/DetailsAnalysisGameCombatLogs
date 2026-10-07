using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.Hubs.Hubs;

[Authorize]
public class PersonalChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatHub> _logger;

    public PersonalChatHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<PersonalChatHub> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
    }

    public async Task JoinRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            await Groups.AddToGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join chat to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task CreateChat(string initiatorId, string companionId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(initiatorId, nameof(initiatorId));
            ArgumentNullException.ThrowIfNullOrEmpty(companionId, nameof(companionId));

            var personalChat = new PersonalChatModel
            {
                InitiatorId = initiatorId,
                CompanionId = companionId
            };

            var response = await _httpClient.PostAsync("PersonalChat", JsonContent.Create(personalChat));
            response.EnsureSuccessStatusCode();

            var createdChat = await response.Content.ReadFromJsonAsync<PersonalChatModel>();
            ArgumentNullException.ThrowIfNull(createdChat, nameof(createdChat));

            await Clients.Caller.SendAsync("ReceivePersonalChat", createdChat);
            await Clients.Group(companionId).SendAsync("ReceivePersonalChat", createdChat);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Leave from room failed: Parameter '{ParamName}' was null.", ex.ParamName);
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
