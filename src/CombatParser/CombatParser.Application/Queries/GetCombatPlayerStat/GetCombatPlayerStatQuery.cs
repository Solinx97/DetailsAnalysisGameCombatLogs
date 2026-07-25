using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStat;

public record GetCombatPlayerStatQuery(
    int CombatPlayerId
    ) : IRequest<CombatPlayerStatsDto>;