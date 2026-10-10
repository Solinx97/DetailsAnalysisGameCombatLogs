using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CombatAnalysis.ChatAPI.Providers;

internal sealed class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId;
    }
}
