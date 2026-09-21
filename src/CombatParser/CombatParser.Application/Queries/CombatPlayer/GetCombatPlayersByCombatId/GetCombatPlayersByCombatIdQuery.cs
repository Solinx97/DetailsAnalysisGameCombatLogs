using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetCombatPlayersByCombatId;

public record GetCombatPlayersByCombatIdQuery(
    int CombatId
    ) : IRequest<IEnumerable<CombatPlayerDto>>;
