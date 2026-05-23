using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetAbilitiesByAbilityType;

public record GetAbilitiesByAbilityTypeQuery(
    int AbilityType
    ) : IRequest<IEnumerable<CombatAbilityDto>>;
