using CombatAnalysis.BL.Interfaces.Entity;

namespace CombatAnalysis.BL.DTO;

public class ResourceRecoveryGeneralDto : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public double ResourcePerSecond { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
