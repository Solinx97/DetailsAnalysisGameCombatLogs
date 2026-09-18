namespace CombatAnalysis.WoW.CombatParser.Entities;

public class Combat
{
    public string DungeonName { get; set; } = string.Empty;

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public long ResourcesRecovery { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public List<CombatPlayer> CombatPlayers { get; set; } = [];

    public List<Unit> Units { get; set; } = [];

    public Boss Boss { get; set; } = new();

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }
}