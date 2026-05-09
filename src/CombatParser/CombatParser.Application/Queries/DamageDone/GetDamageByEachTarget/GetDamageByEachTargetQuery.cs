using CombatParser.Application.DTOs;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamageByEachTarget;

public record GetDamageByEachTargetQuery(
    int CombatId
    ) : IRequest<IEnumerable<IEnumerable<CombatTargetDto>>>;