namespace CombatAnalysis.CombatParserAPI.Consts;

internal static class DBConfiguration
{
    public static int CommandTimeout { get; set; } = 180;

    public static int MaxRequestBodySize { get; set; } = 150000000;
}
