using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.ChatAPI.Providers;

internal sealed class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst("user_id")?.Value;

        return userId;
    }
}
