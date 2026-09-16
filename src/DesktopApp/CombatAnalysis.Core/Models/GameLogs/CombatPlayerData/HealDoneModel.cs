using CombatAnalysis.Core.Interfaces.Entities;

namespace CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

public class HealDoneModel : IDetailsEntity
{
    public string Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public UnitModel Unit { get; set; } = new();

    public UnitModel Target { get; set; } = new();

    public int ModificationType { get; set; }

    public string UnitId { get; set; }
}
