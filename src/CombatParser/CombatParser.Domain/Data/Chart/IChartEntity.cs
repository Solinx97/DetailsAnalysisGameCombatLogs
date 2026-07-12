namespace CombatParser.Domain.Data.Chart;

public interface IChartEntity
{
    int Value { get; }

    TimeSpan Time { get; }
}
