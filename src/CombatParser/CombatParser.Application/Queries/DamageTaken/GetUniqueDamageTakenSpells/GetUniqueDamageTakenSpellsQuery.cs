using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetUniqueDamageTakenSpells;

public record GetUniqueDamageTakenSpellsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
