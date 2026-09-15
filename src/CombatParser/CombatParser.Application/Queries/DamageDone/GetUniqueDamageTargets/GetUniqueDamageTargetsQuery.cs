using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;

public record GetUniqueDamageTargetsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;