using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetByIdCombat;

public record GetByIdCombatQuery(
    int Id
    ) : IRequest<CombatDto>;