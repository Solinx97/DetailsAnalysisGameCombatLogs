using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetUniqueTargetsDamageDone;

public record GetUniqueTargetsDamageDoneQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;