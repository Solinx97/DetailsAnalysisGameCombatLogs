using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.GetResourcesGenerals;

public record GetResourcesGeneralsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ResourceRecoveryGeneralDto>>;