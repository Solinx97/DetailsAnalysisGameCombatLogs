using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHPS;

public record GetHPSQuery(
    int CombatLogId,
    int CombatId,
    string UnitName
    ) : IRequest<DashboardDto>;
