using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerByGameId;

public record GetPlayerByGameIdQuery(
    string GameId
    ) : IRequest<PlayerDto>;
