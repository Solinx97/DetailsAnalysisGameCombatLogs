using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatMessageController(IPersonalChatApiClient httpClient) : ControllerBase
{
    private readonly IPersonalChatApiClient _httpClient = httpClient;

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId, CancellationToken cancellationToken)
    {
        var count = await _httpClient.CountMessagesAsync(chatId, cancellationToken);
        return Ok(count);
    }

    [HttpGet("getByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetByChatId(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var messages = await _httpClient.GetMessagesByChatIdAsync(chatId, page, pageSize, cancellationToken);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatMessageModel message, CancellationToken cancellationToken)
    {
        await _httpClient.CreateMessageAsync(message, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] PersonalChatMessagePatch message, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateMessageAsync(id, message, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteMessageAsync(id, cancellationToken);
        return NoContent();
    }
}
