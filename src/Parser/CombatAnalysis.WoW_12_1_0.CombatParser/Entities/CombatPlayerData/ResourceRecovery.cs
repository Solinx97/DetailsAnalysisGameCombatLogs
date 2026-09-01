using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities.CombatPlayerData;

public class ResourceRecovery : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int CombatPlayerId { get; set; }
}
