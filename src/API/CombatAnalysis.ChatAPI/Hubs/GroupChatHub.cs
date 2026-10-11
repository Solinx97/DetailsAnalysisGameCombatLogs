using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class GroupChatHub : Hub
{
}
