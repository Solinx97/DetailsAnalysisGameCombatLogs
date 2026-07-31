using CombatParser.Application.DTOs.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResources;

public record GetResourcesQuery(
    int CombatPlayerId,
    string Target,
    string Creator,
    string Spell,
    string From,
    string To,
    int Page,
    int PageSzie
    ) : IRequest<IEnumerable<ResourceRecoveryDto>>;
