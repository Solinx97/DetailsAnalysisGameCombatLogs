using CombatAnalysis.BL.Interfaces.Entity;

namespace CombatAnalysis.BL.DTO;

public class PlayerParseInfoDto : ICombatPlayerEntity
{
    public int Id { get; set; }

    public int SpecId { get; set; }

    public int ClassId { get; set; }

    public int BossId { get; set; }

    public int Difficult { get; set; }

    public int DamageEfficiency { get; set; }

    public int HealEfficiency { get; set; }

    public int CombatPlayerId { get; set; }
}
