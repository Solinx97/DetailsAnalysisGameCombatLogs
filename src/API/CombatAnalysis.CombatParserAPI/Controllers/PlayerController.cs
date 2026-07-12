using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Commands.CreatePlayer;
using CombatParser.Application.Queries.GetPlayerByGameId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CombatParserAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PlayerController(IMediator mediator, ILogger<PlayerController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<PlayerController> _logger = logger;

    [HttpGet("getByGamePlayerId/{gamePlayerId}")]
    public async Task<IActionResult> GetByGamePlayerId(string gamePlayerId, CancellationToken cancellationToken)
    {
        var player = await _mediator.Send(new GetPlayerByGameIdQuery(gamePlayerId), cancellationToken);

        return Ok(player);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlayerModel player, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid Player create received: {@Player}", player);

                return ValidationProblem(ModelState);
            }

            var createdItem = await _mediator.Send(new CreatePlayerCommand(player.GameId, player.Username, player.Faction), cancellationToken);

            return Ok(createdItem);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create player.");

            return StatusCode(500, "Internal server error.");
        }
    }
}