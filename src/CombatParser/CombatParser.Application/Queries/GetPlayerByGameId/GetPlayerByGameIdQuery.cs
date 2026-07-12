using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerByGameId;

public record GetPlayerByGameIdQuery(
    string GameId
    ) : IRequest<PlayerDto>;
