using Chat.Application.Commands.PersonalChat.CreateMessage;
using Chat.Application.Commands.PersonalChat.DeleteMessage;
using Chat.Application.Commands.PersonalChat.UpdateMessageText;
using Chat.Application.Queries.PersonalChat.CountMessages;
using Chat.Application.Queries.PersonalChat.GetMessagesByChatId;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Patches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class PersonalChatMessageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("count/{chatId:int:min(1)}")]
    public async Task<IActionResult> Count(int chatId, CancellationToken cancellationToken)
    {
        var count = await _mediator.Send(new CountMessagesQuery(chatId), cancellationToken);

        return Ok(count);
    }

    [HttpGet("getByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetByChatId(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var messages = await _mediator.Send(new GetMessagesByChatIdQuery(chatId, page, pageSize), cancellationToken);

        return Ok(messages);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonalChatMessageModel chatMessage, CancellationToken cancellationToken)
    {
        var command = new CreateMessageCommand(chatMessage.Username, chatMessage.Message, chatMessage.PersonalChatId, chatMessage.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartialUpdate(Guid id, [FromBody] PersonalChatMessagePatch chatMessage, CancellationToken cancellationToken)
    {
        if (id != chatMessage.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateMessageTextCommand(chatMessage.Id, chatMessage.Message);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, int chatId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMessageCommand(id, chatId), cancellationToken);

        return NoContent();
    }
}
