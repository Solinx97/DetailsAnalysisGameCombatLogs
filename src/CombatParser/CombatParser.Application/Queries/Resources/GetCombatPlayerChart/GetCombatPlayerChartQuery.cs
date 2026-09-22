using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetCombatPlayerChart;

public record GetCombatPlayerChartQuery(
    string UnitId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
