using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositions;

public record GetUnitPositionsQuery(
    string CombatUnitId
    ) : IRequest<IEnumerable<UnitPositionDto>>;
