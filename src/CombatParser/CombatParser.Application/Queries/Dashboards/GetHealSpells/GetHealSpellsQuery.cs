using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHealSpells;

public record GetHealSpellsQuery(
    int CombatLogId,
    int CombatId,
    string UnitName
    ) : IRequest<DashboardDto>;
