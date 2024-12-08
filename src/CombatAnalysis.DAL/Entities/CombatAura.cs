using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class CombatAura : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int AuraType { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan FinishTime { get; set; }

    public int Stacks { get; set; }

    public int CombatId { get; set; }
}
