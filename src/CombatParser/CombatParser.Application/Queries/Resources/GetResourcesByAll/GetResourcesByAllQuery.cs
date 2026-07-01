using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetResourcesByAll;

public record GetResourcesByAllQuery(
    int CombatPlayerId,
    string Creator,
    string Spell,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<ResourceRecoveryDto>>;