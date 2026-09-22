using CombatParser.Application.Interfaces;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetCombatPlayerStatById;

public record GetCombatPlayerStatByIdQuery(
    int Id
    ) : IRequest<IPlayerStatsDto>;
