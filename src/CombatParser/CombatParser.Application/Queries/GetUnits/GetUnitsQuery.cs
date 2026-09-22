using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnits;

public record GetUnitsQuery(
    int CombatId
    ) : IRequest<IEnumerable<UnitDto>>;
