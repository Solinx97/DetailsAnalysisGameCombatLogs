using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Chat;

[Route("api/v1/[controller]")]
[ApiController]
public class GroupChatUserController(IGroupChatApiClient httpClient) : ControllerBase
{
    private readonly IGroupChatApiClient _httpClient = httpClient;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _httpClient.GetUserByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpGet("findChatUser/{appUserId}")]
    public async Task<IActionResult> FindChatUser(Guid appUserId, int chatId, CancellationToken cancellationToken)
    {
        var user = await _httpClient.FindChatUserAsync(appUserId, chatId, cancellationToken);
        return Ok(user);
    }

    [HttpGet("findAllChatUsers/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindAllChatUsers(int chatId, CancellationToken cancellationToken)
    {
        var users = await _httpClient.FindAllChatUsersAsync(chatId, cancellationToken);
        return Ok(users);
    }


    [HttpGet("findChatUsers/{appUserId}")]
    public async Task<IActionResult> FindChatUsers(Guid appUserId, CancellationToken cancellationToken)
    {
        var users = await _httpClient.FindChatUsersAsync(appUserId, cancellationToken);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupChatUserModel user, CancellationToken cancellationToken)
    {
        await _httpClient.AddUserAsync(user, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> LeaveFromChat(Guid id, int chatId, CancellationToken cancellationToken)
    {
        await _httpClient.LeaveFromChatAsync(id, chatId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("deleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, int chatId, Guid whoDeleteId, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteChatUserAsync(id, chatId, whoDeleteId, cancellationToken);
        return NoContent();
    }
}
