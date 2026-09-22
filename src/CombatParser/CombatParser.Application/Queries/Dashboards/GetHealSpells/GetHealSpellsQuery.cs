using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHealSpells;

public record GetHealSpellsQuery(
    int CombatLogId,
    string BossName,
    int CombatId
    ) : IRequest<DashboardDto>;
