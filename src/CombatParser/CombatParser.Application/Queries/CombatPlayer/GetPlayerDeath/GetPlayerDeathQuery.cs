using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerDeath;

public record GetPlayerDeathQuery(
    string UnitId,
    string WhenDied
    ) : IRequest<IEnumerable<CombatPlayerDeathDto>>;
