namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatLogByUserModel
{
    public int Id { get; set; }

    public int PersonalLogType { get; set; }

    public int NumberReadyCombats { get; set; }

    public int CombatsInQueue { get; set; }

    public bool IsReady { get; set; }

    public int CombatLogId { get; set; }

    public string AppUserId { get; set; }
}
