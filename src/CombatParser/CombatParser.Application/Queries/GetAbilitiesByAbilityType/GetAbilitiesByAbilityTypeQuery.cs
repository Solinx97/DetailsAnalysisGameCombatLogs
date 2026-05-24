using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetAbilitiesByAbilityType;

public record GetAbilitiesByAbilityTypeQuery(
    int CombatPlayerId,
    int AbilityType
    ) : IRequest<IEnumerable<CombatAbilityDto>>;
