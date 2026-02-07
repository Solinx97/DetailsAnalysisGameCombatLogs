using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResources;

public record GetResourcesQuery(
    int CombatPlayerId,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<ResourceRecoveryDto>>;
