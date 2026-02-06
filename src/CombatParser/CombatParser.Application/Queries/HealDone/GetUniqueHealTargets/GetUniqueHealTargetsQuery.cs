using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;

public record GetUniqueHealTargetsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;