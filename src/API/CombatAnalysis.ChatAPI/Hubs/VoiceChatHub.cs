using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class VoiceChatHub(ILogger<VoiceChatHub> logger) : Hub
{
    private readonly ILogger<VoiceChatHub> _logger = logger;
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupUsers = new();

    public async Task JoinRoom(string voiceChatId)
    {
        try
        {
            var connectedUserId = await AddUserToRoomAsync(voiceChatId);

            await Groups.AddToGroupAsync(Context.ConnectionId, voiceChatId);

            await Clients.Caller.SendAsync("Connected", connectedUserId);

            await Clients.OthersInGroup(voiceChatId).SendAsync("UserJoined", connectedUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public Task<string[]> GetOtherConnectedUsers(string voiceChatId)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        var users = _groupUsers.TryGetValue(voiceChatId, out var roomUsers)
            ? roomUsers.Where(x => x != userId).ToArray()
            : [];

        return Task.FromResult(users ?? []);
    }

    public async Task SendOffer(string voiceChatId, string offer)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveOffer", userId, offer);
    }

    public async Task SendAnswer(string voiceChatId, string answer)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveAnswer", userId, answer);
    }

    public async Task SendCandidate(string voiceChatId, string candidate)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveCandidate", userId, candidate);
    }

    public async Task SendMicrophoneStatus(string voiceChatId, bool status)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveMicrophoneStatus", userId, status);
    }

    public async Task SendRequestMicrophoneStatus(string voiceChatId)
    {
        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveRequestMicrophoneStatus");
    }

    public async Task SendCameraStatus(string voiceChatId, bool status)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveCameraStatus", userId, status);
    }

    public async Task SendRequestCameraStatus(string voiceChatId)
    {
        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveRequestCameraStatus");
    }

    public async Task SendScreenSharingStatus(string voiceChatId, bool status)
    {
        var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveScreenSharingStatus", userId, status);
    }

    public async Task SendRequestScreenSharingStatus(string voiceChatId)
    {
        await Clients.OthersInGroup(voiceChatId).SendAsync("ReceiveRequestScreenSharingStatus");
    }

    public async Task LeaveRoom(string voiceChatId)
    {
        try
        {
            var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

            if (_groupUsers.TryGetValue(voiceChatId, out var users))
            {
                lock (users)
                {
                    users.Remove(userId);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, voiceChatId);

            await Clients.OthersInGroup(voiceChatId).SendAsync("UserLeft", userId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
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

    private async Task<string> AddUserToRoomAsync(string voiceChatId)
    {
        try
        {
            var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

            var users = _groupUsers.GetOrAdd(voiceChatId, _ => []);
            lock (users)
            {
                users.Add(userId);
            }

            return userId;
        }
        catch
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, voiceChatId);

            throw;
        }
    }
}

