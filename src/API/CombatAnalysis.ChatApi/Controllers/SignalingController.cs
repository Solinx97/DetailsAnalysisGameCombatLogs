using CombatAnalysis.ChatApi.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SignalingController : ControllerBase
{
    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastMessage([FromBody] string message)
    {
        await WebSocketMiddleware.BroadcastMessageAsync(message);

        return Ok();
    }
}
