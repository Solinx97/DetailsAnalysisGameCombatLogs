using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetAurasByCombatId;

public record GetAurasByCombatIdQuery(
    int CombatId
    ) : IRequest<IEnumerable<CombatPlayerAuraDto>>;
