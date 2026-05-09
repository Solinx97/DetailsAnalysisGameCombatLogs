using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayersByCombatId;

public record GetCombatPlayersByCombatIdQuery(
    int CombatId
    ) : IRequest<IEnumerable<CombatPlayerDto>>;
