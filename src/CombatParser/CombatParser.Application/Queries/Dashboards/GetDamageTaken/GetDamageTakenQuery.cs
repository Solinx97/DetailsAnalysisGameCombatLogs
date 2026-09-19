using CombatParser.Application.DTOs.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageTaken;

public record GetDamageTakenQuery(
    int CombatLogId,
    int CombatId,
    string UnitName,
    int ValueType
    ) : IRequest<DashboardDto>;
