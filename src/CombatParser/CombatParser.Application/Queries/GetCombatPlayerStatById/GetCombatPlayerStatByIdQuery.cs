using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStatById;

public record GetCombatPlayerStatByIdQuery(
    int Id
    ) : IRequest<CombatPlayerStatsDto>;
