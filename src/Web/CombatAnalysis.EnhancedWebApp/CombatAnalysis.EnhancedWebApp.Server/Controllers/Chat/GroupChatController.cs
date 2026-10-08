using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatController(IGroupChatApiClient httpClient) : ControllerBase
{
    private readonly IGroupChatApiClient _httpClient = httpClient;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var chat = await _httpClient.GetByIdAsync(id, cancellationToken);
        return Ok(chat);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.CreateAsync(chat, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] GroupChatPatch chat, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateChatNameAsync(id, chat, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteChatAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPut("updateRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> UpdateRules(int chatId, [FromBody] GroupChatRulesModel chatRules, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateRulesAsync(chatId, chatRules, cancellationToken);
        return NoContent();
    }

    [HttpGet("getRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetRules(int chatId, CancellationToken cancellationToken)
    {
        var rules = await _httpClient.GetRulesAsync(chatId, cancellationToken);
        return Ok(rules);
    }
}
