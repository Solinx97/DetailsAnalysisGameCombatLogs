using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.ChatAPI.Hubs;

[Authorize]
public class PersonalChatMessagesHub(ILogger<PersonalChatMessagesHub> logger) : Hub
{
    private readonly ILogger<PersonalChatMessagesHub> _logger = logger;

    public async Task JoinToRoom(int chatId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(chatId, nameof(chatId));

            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                throw new HubException("User is not authenticated.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(int room)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(room, nameof(room));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument. Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
    }
}
