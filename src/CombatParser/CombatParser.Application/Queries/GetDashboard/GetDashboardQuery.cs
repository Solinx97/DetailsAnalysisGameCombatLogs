using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.GetDashboard;

public record GetDashboardQuery(
    int CombatLogId
    ) : IRequest<DashboardDto[]>;
