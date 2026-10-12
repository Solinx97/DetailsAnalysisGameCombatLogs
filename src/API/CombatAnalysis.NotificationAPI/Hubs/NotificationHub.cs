using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.NotificationAPI.Hubs;

[Authorize]
public class NotificationHub : Hub
{
}
