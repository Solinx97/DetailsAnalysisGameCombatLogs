using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueDamageTargets;

public record GetUniqueDamageTargetsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;