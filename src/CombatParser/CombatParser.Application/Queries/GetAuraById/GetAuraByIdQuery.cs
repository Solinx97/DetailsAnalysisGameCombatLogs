using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetAuraById;

public record GetAuraByIdQuery(
    int Id
    ) : IRequest<CombatPlayerAuraDto>;
