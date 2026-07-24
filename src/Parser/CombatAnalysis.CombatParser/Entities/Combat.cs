namespace CombatAnalysis.CombatParser.Entities;

public class Combat
{
    public string DungeonName { get; set; } = string.Empty;

    public string[] Data { get; set; } = [];

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public List<CombatPlayer> CombatPlayers { get; set; } = [];

    public List<UnitHealth> UnitHealths { get; set; } = [];

    public Dictionary<string, List<string>> PetsId { get; set; } = [];

    public Boss Boss { get; set; } = new();

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }
}