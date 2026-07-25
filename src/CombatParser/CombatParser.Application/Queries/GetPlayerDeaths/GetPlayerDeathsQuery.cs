using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerDeaths;

public record GetPlayerDeathsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerDeathDto>>;