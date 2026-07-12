using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStat;

public record GetCombatPlayerStatQuery(
    int CombatPlayerId
    ) : IRequest<CombatPlayerStatsDto>;