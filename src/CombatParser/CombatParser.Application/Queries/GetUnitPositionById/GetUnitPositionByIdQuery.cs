using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositionById;

public record GetUnitPositionByIdQuery(
    string Id
    ) : IRequest<UnitPositionDto>;