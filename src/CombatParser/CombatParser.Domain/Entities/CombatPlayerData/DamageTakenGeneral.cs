using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageTakenGeneral
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    [MaxLength(126)]
    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public int ActualValue { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public int CritNumber { get; set; }

    public int MissNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
