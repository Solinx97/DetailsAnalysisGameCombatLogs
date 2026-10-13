using Chat.Application.Commands.PersonalChat.CreateMessage;
using Chat.Application.Commands.PersonalChat.DeleteMessage;
using Chat.Application.Commands.PersonalChat.ReadMessage;
using Chat.Application.Commands.PersonalChat.UpdateMessageMark;
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
        var command = new CreateMessageCommand(chatMessage.Id, chatMessage.Username, chatMessage.Message, chatMessage.PersonalChatId, chatMessage.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("updateMessageText/{id}")]
    public async Task<IActionResult> UpdateMessageText(Guid id, [FromQuery] string newMessage, CancellationToken cancellationToken)
    {
        var command = new UpdateMessageTextCommand(id, newMessage);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("updateMessageMark/{id}")]
    public async Task<IActionResult> UpdateMessageMark(Guid id, [FromQuery] int newMark, CancellationToken cancellationToken)
    {
        var command = new UpdateMessageMarkCommand(id, newMark);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("read/{id}")]
    public async Task<IActionResult> ReadMessage(Guid id, [FromBody] PersonalChatMessageReadPatch chatMessage, CancellationToken cancellationToken)
    {
        if (id != chatMessage.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new ReadMessageCommand(chatMessage.Id, chatMessage.ChatId, chatMessage.ReaderId);
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
