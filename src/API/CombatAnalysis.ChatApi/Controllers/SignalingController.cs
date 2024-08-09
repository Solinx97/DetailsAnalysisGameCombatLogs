using CombatAnalysis.ChatApi.Core;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    [HttpGet("connected/{roomId}")]
    public IActionResult GetConnectedUsers(int roomId)
    {
        var connectedUsers = WebSocketConnectionManager.GetConnectedUsers(roomId);

        return Ok(connectedUsers);
    }
}
