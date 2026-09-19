using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamage;

public record GetDamageQuery(
    int CombatLogId,
    int CombatId,
    string UnitName,
    int ValueType
    ) : IRequest<DashboardDto>;
