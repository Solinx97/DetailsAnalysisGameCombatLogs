using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerCasts;

public record GetCombatPlayerCastsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerCastDto>>;
