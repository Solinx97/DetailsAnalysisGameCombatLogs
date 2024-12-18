namespace CombatAnalysis.CombatParserAPI.Consts;

internal class DBConfiguration
{
    public int CommandTimeout { get; set; } = 180;

    public int MaxRequestBodySize { get; set; } = 150000000;
}
