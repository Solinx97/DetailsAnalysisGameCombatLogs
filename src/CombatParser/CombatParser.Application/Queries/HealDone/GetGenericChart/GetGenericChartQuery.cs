using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetGenericChart;

public record GetGenericChartQuery(
    int CombatId
    ) : IRequest<Dictionary<string, ChartGenericDto[]>>;