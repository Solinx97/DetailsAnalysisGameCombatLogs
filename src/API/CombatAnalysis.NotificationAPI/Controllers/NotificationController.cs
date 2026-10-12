using CombatAnalysis.NotificationAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands.CreateNotification;
using Notification.Application.Commands.DeleteNotification;
using Notification.Application.Queries.GetNotificationById;
using Notification.Application.Queries.GetNotificationByRecipientId;
using Notification.Application.Queries.GetUnreadNotification;

namespace CombatAnalysis.NotificationAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class NotificationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var notification = await _mediator.Send(new GetNotificationByIdQuery(id), cancellationToken);

        return Ok(notification);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationModel notification, CancellationToken cancellationToken)
    {
        var command = new CreateNotificationCommand(notification.Type, notification.InitiatorId,
            notification.RecipientId, notification.InitiatorName);
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
