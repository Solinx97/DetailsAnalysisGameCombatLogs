using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerCasts;

public record GetCombatPlayerCastsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<CombatPlayerCastDto>>;
