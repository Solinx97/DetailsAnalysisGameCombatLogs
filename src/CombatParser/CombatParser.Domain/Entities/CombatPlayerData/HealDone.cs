using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class HealDone
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    [MaxLength(126)]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int Overheal { get; set; }

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
