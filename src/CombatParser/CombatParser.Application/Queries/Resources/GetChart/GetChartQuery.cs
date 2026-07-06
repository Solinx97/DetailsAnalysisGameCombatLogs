using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetChart;

public record GetChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
