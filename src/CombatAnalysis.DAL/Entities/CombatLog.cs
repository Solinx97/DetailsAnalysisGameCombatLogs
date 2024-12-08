using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class CombatLog : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public int NumberReadyCombats { get; set; }

    public int CombatsInQueue { get; set; }

    public bool IsReady { get; set; }
}