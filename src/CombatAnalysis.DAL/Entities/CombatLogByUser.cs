namespace CombatAnalysis.DAL.Entities;

public class CombatLogByUser
{
    public int Id { get; set; }

    public int CombatLogId { get; set; }

    public string AppUserId { get; set; }

    public int PersonalLogType { get; set; }
}
