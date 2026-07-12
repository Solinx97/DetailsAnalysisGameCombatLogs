using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetGenericChart;

public record GetGenericChartQuery(
    int CombatId
    ) : IRequest<Dictionary<string, ChartGenericDto[]>>;