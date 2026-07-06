using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetChart;

public record GetChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
