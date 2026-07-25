using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerPositionById;

public record GetCombatPlayerPositionByIdQuery(
    string Id
    ) : IRequest<CombatPlayerPositionDto>;