using Chat.Application.Commands.PersonalChat.CreateChat;
using Chat.Application.Commands.PersonalChat.DeleteChat;
using Chat.Application.Queries.GroupChat.GetById;
using Chat.Application.Queries.PersonalChat.GetByUserId;
using Chat.Application.Queries.PersonalChat.IsChatExist;
using CombatAnalysis.ChatAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PersonalChatController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var chat = await _mediator.Send(new GetByIdQuery(id), cancellationToken);

        return Ok(chat);
    }

    [HttpGet("getByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var chats = await _mediator.Send(new GetByUserIdQuery(userId), cancellationToken);

        return Ok(chats);
    }

    [HttpGet("isExist")]
    public async Task<IActionResult> IsChatExist(Guid initiatorId, Guid companionId, CancellationToken cancellationToken)
    {
        var chats = await _mediator.Send(new IsChatExistQuery(initiatorId, companionId), cancellationToken);

        return Ok(chats);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatModel personalChat, CancellationToken cancellationToken)
    {
        var command = new CreateChatCommand(personalChat.InitiatorId, personalChat.CompanionId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChatCommand(id), cancellationToken);

        return NoContent();
    }
}
