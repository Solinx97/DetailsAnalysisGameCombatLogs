using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenCreators;

public record GetUniqueDamageTakenCreatorsQuery(
    string UnitId
    ) : IRequest<IEnumerable<string>>;