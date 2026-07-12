using CombatParser.Application.DTOs.Chart;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetCombatPlayerChart;

public record GetCombatPlayerChartQuery(
    int CombatPlayerId
    ) : IRequest<IEnumerable<ChartGenericDto>>;
