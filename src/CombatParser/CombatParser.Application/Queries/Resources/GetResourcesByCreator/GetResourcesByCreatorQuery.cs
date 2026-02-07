using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesByCreator;

public record GetResourcesByCreatorQuery(
    int CombatPlayerId,
    string Target,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<ResourceRecoveryDto>>;