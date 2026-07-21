using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities.CombatPlayerData;

public class HealDone : ICombatPlayerEntity
{
    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public int CombatPlayerId { get; set; }
}