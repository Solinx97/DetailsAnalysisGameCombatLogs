using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamage;

public record GetDamageQuery(
    int CombatLogId,
    string BossName,
    int CombatId,
    string CreatorName,
    string TargetName,
    int ValueType
    ) : IRequest<DashboardDto>;
