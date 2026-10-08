using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatMessageController(IGroupChatApiClient httpClient) : ControllerBase
{
    private readonly IGroupChatApiClient _httpClient = httpClient;

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountAsync(chatId, cancellationToken);
        return Ok(count);
    }

    [HttpGet("getByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetByChatId(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var messages = await _httpClient.GetByChatIdAsync(chatId, page, pageSize, cancellationToken);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GroupChatMessageModel message, CancellationToken cancellationToken)
    {
        var cxreatedMessage = await _httpClient.CreateAsync(message, cancellationToken);
        return Ok(cxreatedMessage);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(string id, [FromBody] GroupChatMessagePatch message, CancellationToken cancellationToken)
    {
        await _httpClient.PatchAsync(id, message, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
