using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHeal;

public record GetHealQuery(
    int CombatLogId,
    int CombatId,
    string UnitName,
    int ValueType
    ) : IRequest<DashboardDto>;
