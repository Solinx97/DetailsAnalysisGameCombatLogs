using CombatParser.Domain.Data.Chart;

namespace CombatParser.Domain.Entities.Chart;

public record ChartGeneric(
    int Value,
    TimeSpan Time
    ) : IChartEntity;