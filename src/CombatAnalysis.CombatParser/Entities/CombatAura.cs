using CombatAnalysis.CombatParser.Enums;

namespace CombatAnalysis.CombatParser.Entities;

public class CombatAura
{
    public string Name { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public AuraType AuraType { get; set; }

    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; }

    public int Stacks { get; set; }

    public int CombatId { get; set; }
}
