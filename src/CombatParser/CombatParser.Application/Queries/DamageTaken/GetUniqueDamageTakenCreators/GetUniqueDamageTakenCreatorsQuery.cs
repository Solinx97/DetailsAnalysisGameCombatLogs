using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenCreators;

public record GetUniqueDamageTakenCreatorsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;