using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;

public record GetUniqueHealSpellsQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<string>>;
