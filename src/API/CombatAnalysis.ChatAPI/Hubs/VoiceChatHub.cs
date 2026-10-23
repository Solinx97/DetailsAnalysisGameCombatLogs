using CombatAnalysis.ChatAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class VoiceChatHub(ILiveKitService liveKitService) : Hub
{
    private readonly ILiveKitService _liveKitService = liveKitService;
    private static readonly ConcurrentDictionary<string, HashSet<string>> _groupUsers = new();

    public async Task JoinToRoom(string chatId)
    {
        await AddUserToRoomAsync(chatId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        await SendParticipantsCountAsync(chatId);
    }

    public async Task<int> SubscribeToVoiceChatUsersCount(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"VoiceChatCount_{chatId}");

        var count = await _liveKitService.GetParticipantsCountAsync(chatId);
        return await Task.FromResult(count);
    }

    public async Task UnsubscribeFromVoiceChatUsersCount(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"VoiceChatCount_{chatId}");
    }

    public async Task LeaveFromRoom(string chatId)
    {
        var userId = Context.UserIdentifier
            ?? throw new HubException("User is not authenticated.");

        if (_groupUsers.TryGetValue(chatId, out var users))
        {
            lock (users)
            {
                users.Remove(userId);
            }
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

        await SendParticipantsCountAsync(chatId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var group in _groupUsers)
        {
            if (group.Value.Contains(Context.ConnectionId))
            {
                await LeaveFromRoom(group.Key);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task AddUserToRoomAsync(string chatId)
    {
        try
        {
            var userId = Context.UserIdentifier
                ?? throw new HubException("User is not authenticated.");

            var users = _groupUsers.GetOrAdd(chatId, _ => []);
            lock (users)
            {
                users.Add(userId);
            }
        }
        catch
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);

            throw;
        }
    }

    private async Task SendParticipantsCountAsync(string chatId)
    {
        var count = await _liveKitService.GetParticipantsCountAsync(chatId);
        await Clients.Group($"VoiceChatCount_{chatId}").SendAsync("ConnectedUsersCount", count);
    }
}

