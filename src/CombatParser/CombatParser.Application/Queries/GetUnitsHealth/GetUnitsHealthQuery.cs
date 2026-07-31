using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitsHealth;

public record GetUnitsHealthQuery(
    int CombatId
    ) : IRequest<IDictionary<string, IEnumerable<UnitHealthDto>>>;
