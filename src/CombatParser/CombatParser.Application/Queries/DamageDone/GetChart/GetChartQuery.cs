using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetChart;

public record GetChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
