using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenCreators;

public record GetUniqueDamageTakenCreatorsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;