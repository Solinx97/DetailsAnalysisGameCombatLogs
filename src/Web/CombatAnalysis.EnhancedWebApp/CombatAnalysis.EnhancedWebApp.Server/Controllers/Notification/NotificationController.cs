using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Notification;
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

    [HttpPatch("read/{id:int:min(1)}")]
    public async Task<IActionResult> ReadNotificaiton(int id, [FromBody] ReadNotificationModel notification, CancellationToken cancellationToken)
    {
        await _httpClient.ReadNotificationAsync(id, notification, cancellationToken);
        return NoContent();
    }

    [HttpPatch("readAll/{recipientId}")]
    public async Task<IActionResult> ReadAllNotificaitons(Guid recipientId, CancellationToken cancellationToken)
    {
        await _httpClient.ReadAllNotificationsAsync(recipientId, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteNotificationAsync(id, cancellationToken);
        return NoContent();
    }
}
