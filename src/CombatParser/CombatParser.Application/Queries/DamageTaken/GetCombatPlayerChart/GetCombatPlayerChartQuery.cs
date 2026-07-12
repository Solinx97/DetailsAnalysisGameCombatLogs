using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetCombatPlayerChart;

public record GetCombatPlayerChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
