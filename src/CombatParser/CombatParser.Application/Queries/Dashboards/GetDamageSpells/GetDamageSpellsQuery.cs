using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageSpells;

public record GetDamageSpellsQuery(
    int CombatLogId
    ) : IRequest<Dictionary<string, int>>;
