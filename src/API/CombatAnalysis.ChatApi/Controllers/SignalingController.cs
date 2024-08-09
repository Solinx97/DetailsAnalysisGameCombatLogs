using CombatAnalysis.ChatApi.Core;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    [HttpGet("connected")]
    public IActionResult GetConnectedUsers()
    {
        var connectedUsers = WebSocketConnectionManager.GetOnlyConnectedUsers();

        return Ok(connectedUsers);
    }
}
