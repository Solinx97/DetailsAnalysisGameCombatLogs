using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class PlayerDeath : ICombatPlayerEntity
{
    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
