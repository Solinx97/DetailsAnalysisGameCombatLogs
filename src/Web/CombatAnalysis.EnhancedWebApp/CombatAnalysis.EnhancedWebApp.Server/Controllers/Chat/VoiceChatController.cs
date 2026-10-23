using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class VoiceChatController(IVoiceChatApiClient httpClient) : ControllerBase
{
    private readonly IVoiceChatApiClient _httpClient = httpClient;

    [HttpGet("isChatExist/{chatId:int:min(1)}")]
    public async Task<IActionResult> IsChatExist(int chatId, CancellationToken cancellationToken)
    {
        var isExist = await _httpClient.IsChatExistAsync(chatId, cancellationToken);
        return Ok(isExist);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoiceChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(chat, cancellationToken);
        return NoContent();
    }

    [HttpPost("token/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetToken(int chatId, [FromBody] GetVoiceChatTokenRequest request, CancellationToken cancellationToken)
    {
        var tokenResponse = await _httpClient.GetTokenAsync(chatId, request, cancellationToken);
        return Ok(tokenResponse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteChatAsync(id, cancellationToken);
        return NoContent();
    }
}
