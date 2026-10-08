using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class PersonalChatController(IPersonalChatApiClient httpClient) : ControllerBase
{
    private readonly IPersonalChatApiClient _httpClient = httpClient;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var chat = await _httpClient.GetByIdAsync(id, cancellationToken);
        return Ok(chat);
    }

    [HttpGet("getByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var chats = await _httpClient.GetByUserIdAsync(userId, cancellationToken);
        return Ok(chats);
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsExist(Guid initiatorId, Guid companionId, CancellationToken cancellationToken)
    {
        var isExist = await _httpClient.IsChatExistAsync(initiatorId, companionId, cancellationToken);
        return Ok(isExist);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(chat, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteChatAsync(id, cancellationToken);
        return NoContent();
    }
}
