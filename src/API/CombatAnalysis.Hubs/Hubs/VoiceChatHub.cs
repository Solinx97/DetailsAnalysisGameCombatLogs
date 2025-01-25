using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CombatAnalysis.Hubs.Hubs;

public class VoiceChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<VoiceChatHub> _logger;
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupUsers = new();

    public VoiceChatHub(IHttpClientHelper httpClient, ILogger<VoiceChatHub> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task JoinRoom(string room, string userId)
    {
        try
        {
            var voiceChat = new VoiceChatModel
            {
                Id = Context.ConnectionId,
                AppUserId = userId
            };

            var response = await _httpClient.PostAsync("VoiceChat", JsonContent.Create(voiceChat));
            response.EnsureSuccessStatusCode();

            var users = _groupUsers.GetOrAdd(room, _ => new HashSet<string>());
            lock (users)
            {
                users.Add(Context.ConnectionId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await Clients.OthersInGroup(room).SendAsync("UserJoined", Context.ConnectionId);
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

    public async Task SendOffer(string room, string userId, string offer)
    {
        await Clients.Client(userId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
    }

    public async Task SendAnswer(string room, string userId, string answer)
    {
        await Clients.Client(userId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }

    public async Task SendCandidate(string room, string userId, string candidate)
    {
        await Clients.Client(room).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
    }

    public async Task SendMicrophoneStatus(string room, bool status)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveMicrophoneStatus", Context.ConnectionId, status);
    }

    public async Task SendRequestMicrophoneStatus(string room)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveRequestMicrophoneStatus", Context.ConnectionId);
    }

    public async Task SendCameraStatus(string room, bool status)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveCameraStatus", Context.ConnectionId, status);
    }

    public async Task SendRequestCameraStatus(string room)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveRequestCameraStatus", Context.ConnectionId);
    }

    public async Task SendScreenSharingStatus(string room, bool status)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveScreenSharingStatus", Context.ConnectionId, status);
    }

    public async Task SendRequestScreenSharingStatus(string room)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveRequestScreenSharingStatus", Context.ConnectionId);
    }

    public async Task RequestConnectedUsers(string room)
    {
        if (_groupUsers.TryGetValue(room, out var users))
        {
            await Clients.Caller.SendAsync("ReceiveConnectedUsers", users);
        }
    }

    public async Task LeaveRoom(string room)
    {
        try
        {
            if (_groupUsers.TryGetValue(room, out var users))
            {
                lock (users)
                {
                    users.Remove(Context.ConnectionId);
                }

                var response = await _httpClient.DeletAsync($"VoiceChat/{Context.ConnectionId}");
                response.EnsureSuccessStatusCode();
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            await Clients.Group(room).SendAsync("UserLeft", Context.ConnectionId);
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

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var group in _groupUsers)
        {
            if (group.Value.Contains(Context.ConnectionId))
            {
                await LeaveRoom(group.Key);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
