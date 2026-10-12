using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.NotificationAPI.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public async Task JoinToRoom()
    {
        var userId = Context.UserIdentifier
            ?? throw new HubException("User is not authenticated.");

        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
    }

    public async Task LeaveFromRoom()
    {
        var userId = Context.UserIdentifier
            ?? throw new HubException("User is not authenticated.");

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
    }
}
