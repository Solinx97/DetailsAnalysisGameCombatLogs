using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealTargets;

public record GetUniqueHealTargetsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;