using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDPS;

public record GetDPSQuery(
    int CombatLogId,
    int CombatId,
    string unitName
    ) : IRequest<DashboardDto>;
