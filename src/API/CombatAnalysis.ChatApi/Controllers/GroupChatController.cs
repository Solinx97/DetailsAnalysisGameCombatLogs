using Chat.Application.Commands.GroupChat.CreateChat;
using Chat.Application.Commands.GroupChat.DeleteChat;
using Chat.Application.Commands.GroupChat.UpdateChatName;
using Chat.Application.Commands.GroupChat.UpdateChatRules;
using Chat.Application.Queries.GroupChat.GetById;
using Chat.Application.Queries.GroupChat.GetRules;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Patches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var chat = await _mediator.Send(new GetByIdQuery(id), cancellationToken);

        return Ok(chat);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupChatModel groupChat, CancellationToken cancellationToken)
    {
        var command = new CreateChatCommand(groupChat.Name, groupChat.OwnerUsername,
            groupChat.InvitePeopleRule, groupChat.RemovePeopleRule, groupChat.PinMessageRule, groupChat.AnnouncementsRule, groupChat.OwnerId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPatch("updateName/{id:int:min(1)}")]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] GroupChatPatch chat, CancellationToken cancellationToken)
    {
        if (id != chat.Id)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateChatNameCommand(id, chat.Name);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChatCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpPut("updateRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> UpdateRules(int chatId, [FromBody] GroupChatRulesModel rules, CancellationToken cancellationToken)
    {
        if (chatId != rules.GroupChatId)
        {
            return BadRequest("Route ID and body ID do not match.");
        }

        var command = new UpdateChatRulesCommand(chatId, rules.InvitePeople, rules.RemovePeople, rules.PinMessage, rules.Announcements);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("getRules/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetRules(int chatId, CancellationToken cancellationToken)
    {
        var chat = await _mediator.Send(new GetRulesQuery(chatId), cancellationToken);

        return Ok(chat);
    }
}
