using CombatParser.Application.Interfaces;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerStats;

public record GetPlayerStatsQuery(
    int Id,
    int GameVersion
    ) : IRequest<IPlayerStatsDto>;
