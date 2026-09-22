using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

public record GetResourcesGeneralsQuery(
    string UnitId,
    int CombatId
    ) : IRequest<IEnumerable<ResourceRecoveryGeneralDto>>;