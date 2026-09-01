namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities;

public class UnitCast
{
    public string CreatorGameId { get; set; } = string.Empty;

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public TimeSpan Time { get; set; }

    public TimeSpan FinishTime { get; set; }

    public string? TargetGameId { get; set; }

    public bool IsImmediatly { get; set; }

    public bool IsSuccess { get; set; }

    public int CombatId { get; set; }
}
