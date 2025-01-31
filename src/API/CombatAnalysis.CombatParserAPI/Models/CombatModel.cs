namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatModel
{
    public int Id { get; set; }

    public int LocallyNumber { get; set; }

    public string DungeonName { get; set; }

    public string Name { get; set; }

    public int Difficulty { get; set; }

    public List<string> Data { get; set; }

    public int EnergyRecovery { get; set; }

    public long DamageDone { get; set; }

    public long HealDone { get; set; }

    public long DamageTaken { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public List<CombatPlayerModel> Players { get; set; }

    public Dictionary<string, List<string>> PetsId { get; set; }

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }

    public int CombatLogId { get; set; }
}
