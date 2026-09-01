using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities.CombatPlayerData;

public class HealDone : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public int CombatPlayerId { get; set; }
}