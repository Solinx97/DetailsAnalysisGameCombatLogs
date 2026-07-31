using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositions;

public record GetUnitPositionsQuery(
    int CombatId
    ) : IRequest<IDictionary<string, IEnumerable<UnitPositionDto>>>;
