using Chat.Application.Commands.VoiceChat.CreateChat;
using Chat.Application.Commands.VoiceChat.DeleteChat;
using Chat.Application.Queries.VoiceChat.GetById;
using Chat.Application.Queries.VoiceChat.IsChatExist;
using CombatAnalysis.ChatAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class VoiceChatController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("getByChatId/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetByChatId(int chatId, CancellationToken cancellationToken)
    {
        var chat = await _mediator.Send(new GetByChatIdQuery(chatId), cancellationToken);
        if (chat == null)
        {
            return NotFound();
        }

        return Ok(chat);
    }

    [HttpGet("isChatExist/{chatId:int:min(1)}")]
    public async Task<IActionResult> IsChatExist(int chatId, CancellationToken cancellationToken)
    {
        var isChatExist = await _mediator.Send(new IsChatExistQuery(chatId), cancellationToken);

        return Ok(isChatExist);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoiceChatModel voiceChat, CancellationToken cancellationToken)
    {
        var command = new CreateChatCommand(voiceChat.GroupChatId, voiceChat.AppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChatCommand(id), cancellationToken);

        return NoContent();
    }
}
