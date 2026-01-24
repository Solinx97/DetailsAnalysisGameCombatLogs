using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities;

public class CombatPlayerDeath
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(126)]
    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
