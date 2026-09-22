namespace CombatParser.Domain.Consts;

public static class PlayerDeathValue
{
    public static TimeSpan IntervalBeforeDied { get; } = TimeSpan.FromSeconds(10);
}
