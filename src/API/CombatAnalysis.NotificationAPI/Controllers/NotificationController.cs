using CombatAnalysis.NotificationAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands.DeleteNotification;
using Notification.Application.Commands.ReadNotification;
using Notification.Application.Commands.ReadUnreadNotifications;
using Notification.Application.Queries.GetNotificationByRecipientId;
using Notification.Application.Queries.GetUnreadNotification;

namespace CombatAnalysis.NotificationAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class NotificationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetByRecipientId(Guid recipientId, CancellationToken cancellationToken)
    {
        var notifications = await _mediator.Send(new GetNotificationByRecipientIdQuery(recipientId), cancellationToken);

        return Ok(notifications);
    }

    [HttpGet("getUnreadByRecipientId/{recipientId}")]
    public async Task<IActionResult> GetUnreadByRecipientId(Guid recipientId, CancellationToken cancellationToken)
    {
        var notifications = await _mediator.Send(new GetUnreadNotificationQuery(recipientId), cancellationToken);

        return Ok(notifications);
    }

    [HttpPatch("read/{id:int:min(1)}")]
    public async Task<IActionResult> ReadNotificaiton(int id, [FromBody] ReadNotificationModel notification, CancellationToken cancellationToken)
    {
        if (id != notification.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new ReadNotificationCommand(notification.Id);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("readAll/{recipientId}")]
    public async Task<IActionResult> ReadAllNotificaitons(Guid recipientId, CancellationToken cancellationToken)
    {
        var command = new ReadUnreadNotificationsCommand(recipientId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteNotificationCommand(id), cancellationToken);

        return NoContent();
    }
}
