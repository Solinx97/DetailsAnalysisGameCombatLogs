using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class ResourceRecovery : ICombatPlayerEntity
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string SpellOrItem { get; set; }

    public int CombatPlayerId { get; set; }
}
