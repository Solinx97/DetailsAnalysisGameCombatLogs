using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerById;

public record GetCombatPlayerByIdQuery(
    int Id
    ) : IRequest<CombatPlayerDto>;