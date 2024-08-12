using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CombatAnalysis.WebApp.Hubs;

public class VoiceChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupUsers = new();
    
    public async Task JoinRoom(string room)
    {
        var users = _groupUsers.GetOrAdd(room, _ => new HashSet<string>());
        lock (users)
        {
            users.Add(Context.ConnectionId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, room);

        await Clients.Group(room).SendAsync("UserJoined", Context.ConnectionId);
    }

    public async Task RequestConnectionId(string room)
    {
        await Clients.Caller.SendAsync("ReceiveConnectionId", Context.ConnectionId);
    }

    public async Task SendOffer(string room, string offer)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveOffer", offer);
    }

    public async Task SendAnswer(string room, string answer)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveAnswer", answer);
    }

    public async Task SendCandidate(string room, string candidate)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveCandidate", candidate);
    }

    public async Task SendMicrophoneStatus(string room, bool status)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveMicrophoneStatus", Context.ConnectionId, status);
    }

    public async Task RequestMicrophoneStatus(string room)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveRequestMicrophoneStatus", Context.ConnectionId);
    }

    public async Task SendCameraStatus(string room, bool status)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveCameraStatus", Context.ConnectionId, status);
    }

    public async Task RequestCameraStatus(string room)
    {
        await Clients.OthersInGroup(room).SendAsync("ReceiveRequestCameraStatus", Context.ConnectionId);
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
        if (_groupUsers.TryGetValue(room, out var users))
        {
            lock (users)
            {
                users.Remove(Context.ConnectionId);
            }
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        await Clients.Group(room).SendAsync("UserLeft", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
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
