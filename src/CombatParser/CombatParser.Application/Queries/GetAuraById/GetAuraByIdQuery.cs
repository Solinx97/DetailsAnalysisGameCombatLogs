using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetAuraById;

public record GetAuraByIdQuery(
    int Id
    ) : IRequest<CombatPlayerAuraDto>;
