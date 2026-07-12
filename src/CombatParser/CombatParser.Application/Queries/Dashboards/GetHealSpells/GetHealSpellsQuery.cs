using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHealSpells;

public record GetHealSpellsQuery(
    int CombatLogId
    ) : IRequest<Dictionary<string, int>>;
