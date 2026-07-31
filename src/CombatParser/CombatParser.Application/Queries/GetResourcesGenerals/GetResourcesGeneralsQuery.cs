using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

public record GetResourcesGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ResourceRecoveryGeneralDto>>;