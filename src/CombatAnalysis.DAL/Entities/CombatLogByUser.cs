using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class CombatLogByUser : IEntity
{
    public int Id { get; set; }

    public int PersonalLogType { get; set; }

    public int NumberReadyCombats { get; set; }

    public int CombatsInQueue { get; set; }

    public bool IsReady { get; set; }

    public int CombatLogId { get; set; }

    public string AppUserId { get; set; }
}
