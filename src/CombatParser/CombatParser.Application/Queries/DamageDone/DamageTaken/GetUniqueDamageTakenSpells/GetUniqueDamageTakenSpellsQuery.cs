using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetUniqueDamageTakenSpells;

public record GetUniqueDamageTakenSpellsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
