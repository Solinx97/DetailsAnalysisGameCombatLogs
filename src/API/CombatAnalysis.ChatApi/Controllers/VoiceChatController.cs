using Chat.Application.Commands.VoiceChat.CreateChat;
using Chat.Application.Commands.VoiceChat.DeleteChat;
using Chat.Application.Queries.VoiceChat.IsChatExist;
using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Interfaces;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.ChatAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class VoiceChatController(IMediator mediator, ILiveKitService liveKitService, IOptions<LiveKitOptions> options) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILiveKitService _liveKitService = liveKitService;
    private readonly LiveKitOptions _options = options.Value;

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

    [HttpPost("token/{chatId:int:min(1)}")]
    public async Task<IActionResult> GetToken(int chatId, [FromBody] GetVoiceChatTokenRequest request)
    {
        var userId = User.FindFirst("user_id")?.Value
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var roomName = $"voice-chat-{chatId}";

        var token = _liveKitService.GenerateToken(
            roomName,
            participantIdentity: userId,
            participantName: request.Username);

        var response = new VoiceChatTokenResponse(_options.Url, token);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteChatCommand(id), cancellationToken);

        return NoContent();
    }
}
