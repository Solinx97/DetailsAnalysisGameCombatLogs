using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetPlayerDeath;

public record GetPlayerDeathQuery(
    string UnitId,
    int SkipCount
    ) : IRequest<IEnumerable<CombatPlayerDeathDto>>;
