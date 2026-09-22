using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetUniqueHealSpells;

public record GetUniqueHealSpellsQuery(
    string UniId
    ) : IRequest<IEnumerable<string>>;
