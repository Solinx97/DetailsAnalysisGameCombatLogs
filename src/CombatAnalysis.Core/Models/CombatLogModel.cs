namespace CombatAnalysis.Core.Models;

public class CombatLogModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public int LogType { get; set; }

    public int NumberReadyCombats { get; set; }

    public int CombatsInQueue { get; set; }

    public bool IsReady { get; set; }

    public string AppUserId { get; set; }
}
