using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStatById;

public record GetCombatPlayerStatByIdQuery(
    int Id
    ) : IRequest<CombatPlayerStatsDto>;
