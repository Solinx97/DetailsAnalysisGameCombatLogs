namespace CombatAnalysis.CombatParserAPI.Consts;

public class DBConfiguration
{
    public int CommandTimeout { get; set; } = 102;

    public int MaxRequestBodySize { get; set; } = 150000000;
}
