using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

public record GetResourcesGeneralsQuery(
    string UnitId
    ) : IRequest<IEnumerable<ResourceRecoveryGeneralDto>>;