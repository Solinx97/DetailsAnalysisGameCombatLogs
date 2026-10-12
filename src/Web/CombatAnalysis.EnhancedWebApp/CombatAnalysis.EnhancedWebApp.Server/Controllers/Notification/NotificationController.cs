using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Notification;

[Route("api/v1/[controller]")]
[ApiController]
public class NotificationController(INotificationApiClient httpClient) : ControllerBase
{
    private readonly INotificationApiClient _httpClient = httpClient;

    [HttpGet("getByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetByRecipientId(Guid recipientId, CancellationToken cancellationToken)
    {
        var notifications = await _httpClient.GetByRecipientIdAsync(recipientId, cancellationToken);
        return Ok(notifications);
    }

    [HttpGet("getUnreadByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetUnreadByRecipientId(Guid recipientId, CancellationToken cancellationToken)
    {
        var notifications = await _httpClient.GetUnreadByRecipientIdAsync(recipientId, cancellationToken);
        return Ok(notifications);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteNotificationAsync(id, cancellationToken);
        return NoContent();
    }
}
