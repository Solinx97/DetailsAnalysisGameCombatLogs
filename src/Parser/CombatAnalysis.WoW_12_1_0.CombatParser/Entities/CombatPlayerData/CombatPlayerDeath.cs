using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerDeath : ICombatPlayerEntity
{
    public string Username { get; set; } = string.Empty;

    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
