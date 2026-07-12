using CombatAnalysis.Core.Interfaces.Entities;

namespace CombatAnalysis.Core.Models.GameLogs;

public class DamageDoneGeneralModel : IGeneralDetailsEntity
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public double DamagePerSecond { get; set; }

    public int CritNumber { get; set; }

    public int MissNumber { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public bool IsPet { get; set; }

    public int CombatPlayerId { get; set; }
}
