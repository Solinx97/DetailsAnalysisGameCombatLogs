using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageSpells;

public record GetDamageSpellsQuery(
    int CombatLogId,
    string BossName,
    int CombatId
    ) : IRequest<DashboardDto>;
