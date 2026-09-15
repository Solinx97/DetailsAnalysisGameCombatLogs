using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetCombatPlayerChart;

public record GetCombatPlayerChartQuery(
    string UnitId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
