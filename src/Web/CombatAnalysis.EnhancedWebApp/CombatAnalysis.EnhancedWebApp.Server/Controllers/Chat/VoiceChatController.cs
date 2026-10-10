using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class VoiceChatController(IVoiceChatApiClient httpClient) : ControllerBase
{
    private readonly IVoiceChatApiClient _httpClient = httpClient;

    [HttpGet("getByChatId/{id:int:min(1)}")]
    public async Task<IActionResult> GetByChatId(int id, CancellationToken cancellationToken)
    {
        var chat = await _httpClient.GetByChatIdAsync(id, cancellationToken);
        return Ok(chat);
    }

    [HttpGet("isChatExist/{id:int:min(1)}")]
    public async Task<IActionResult> IsChatExist(int id, CancellationToken cancellationToken)
    {
        var isExist = await _httpClient.IsChatExistAsync(id, cancellationToken);
        return Ok(isExist);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoiceChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(chat, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteChatAsync(id, cancellationToken);
        return NoContent();
    }
}
