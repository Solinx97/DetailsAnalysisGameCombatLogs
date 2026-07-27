using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatUnits;

public record GetCombatUnitsQuery(
    int CombatId
    ) : IRequest<IEnumerable<CombatUnitDto>>;
