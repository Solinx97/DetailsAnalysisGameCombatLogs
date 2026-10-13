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
    public async Task<IActionResult> Create([FromBody] GroupChatMessageModel message, CancellationToken cancellationToken)
    {
        await _httpClient.CreateMessageAsync(message, cancellationToken);
        return NoContent();
    }

    [HttpPatch("updateMessageText/{id}")]
    public async Task<IActionResult> UpdateMessageText(Guid id, [FromQuery] string newMessage, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateMessageTextAsync(id, newMessage, cancellationToken);
        return NoContent();
    }

    [HttpPatch("updateMessageMark/{id}")]
    public async Task<IActionResult> UpdateMessageMark(Guid id, [FromQuery] int newMark, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateMessageMarkAsync(id, newMark, cancellationToken);
        return NoContent();
    }

    [HttpPatch("read/{id}")]
    public async Task<IActionResult> ReadMessage(Guid id, [FromBody] GroupChatMessageReadPatch message, CancellationToken cancellationToken)
    {
        await _httpClient.ReadMessageIdAsync(id, message, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteMessageAsync(id, cancellationToken);
        return NoContent();
    }
}
