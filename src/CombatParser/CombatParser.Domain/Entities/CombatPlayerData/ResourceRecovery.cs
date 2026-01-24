using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class ResourceRecovery
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    [MaxLength(126)]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    [MaxLength(126)]
    public string Creator { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Target { get; set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
