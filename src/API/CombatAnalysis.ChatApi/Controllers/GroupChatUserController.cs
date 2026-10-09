using Chat.Application.Commands.GroupChat.AddUser;
using Chat.Application.Commands.GroupChat.DeleteUser;
using Chat.Application.Queries.GroupChat.FindAllChatUsers;
using Chat.Application.Queries.GroupChat.FindChatUser;
using Chat.Application.Queries.GroupChat.FindChatUsers;
using Chat.Application.Queries.GroupChat.GetUserById;
using CombatAnalysis.ChatAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class GroupChatUserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        return Ok(user);
    }

    [HttpGet("findChatUser/{appUserId}")]
    public async Task<IActionResult> FindChatUser(Guid appUserId, int chatId, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new FindChatUserQuery(appUserId, chatId), cancellationToken);

        return Ok(user);
    }

    [HttpGet("findAllChatUsers/{chatId:int:min(1)}")]
    public async Task<IActionResult> FindAllChatUsers(int chatId, CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new FindAllChatUsersQuery(chatId), cancellationToken);

        return Ok(users);
    }

    [HttpGet("findChatUsers/{appUserId}")]
    public async Task<IActionResult> FindChatUsers(Guid appUserId, CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new FindChatUsersQuery(appUserId), cancellationToken);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGroupChatUserModel chatUser, CancellationToken cancellationToken)
    {
        var command = new AddUserCommand(chatUser.Username, chatUser.GroupChatId, chatUser.AppUserId, chatUser.WhoAddAppUserId);
        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, int chatId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id, chatId), cancellationToken);

        return NoContent();
    }
}
