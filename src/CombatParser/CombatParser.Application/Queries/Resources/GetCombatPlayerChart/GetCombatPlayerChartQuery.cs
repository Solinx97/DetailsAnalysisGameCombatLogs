using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetCombatPlayerChart;

public record GetCombatPlayerChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
