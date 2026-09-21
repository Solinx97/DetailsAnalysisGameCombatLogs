using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetCombatPlayerById;

public record GetCombatPlayerByIdQuery(
    int Id
    ) : IRequest<CombatPlayerDto>;