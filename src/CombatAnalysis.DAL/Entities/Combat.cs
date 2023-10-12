namespace CombatAnalysis.DAL.Entities;

public class Combat
{
    public int Id { get; set; }

    public int LocallyNumber { get; set; }

    public string DungeonName { get; set; }

    public string Name { get; set; }

    public int Difficulty { get; set; }

    public int DungeonSize { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int EnergyRecovery { get; set; }

    public int DeathNumber { get; set; }

    public bool IsWin { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset FinishDate { get; set; }

    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public bool IsReady { get; set; }

    public int CombatLogId { get; set; }
}
