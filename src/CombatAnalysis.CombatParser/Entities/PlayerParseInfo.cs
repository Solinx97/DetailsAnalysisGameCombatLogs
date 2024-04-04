namespace CombatAnalysis.CombatParser.Entities;

public class PlayerParseInfo
{
    public int SpecId { get; set; } = -1;

    public int ClassId { get; set; } = -1;

    public int BossId { get; set; } = -1;

    public int Difficult { get; set; }

    public int Efficiency { get; set; }

    public int CombatPlayerId { get; set; }
}
