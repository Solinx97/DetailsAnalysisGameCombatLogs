using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerPositionById;

public record GetCombatPlayerPositionByIdQuery(
    int Id
    ) : IRequest<CombatPlayerPositionDto>;