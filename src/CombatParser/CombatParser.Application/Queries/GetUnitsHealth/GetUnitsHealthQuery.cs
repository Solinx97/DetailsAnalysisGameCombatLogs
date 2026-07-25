using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitsHealth;

public record GetUnitsHealthQuery(
    int CombatId
    ) : IRequest<IEnumerable<UnitHealthDto>>;
