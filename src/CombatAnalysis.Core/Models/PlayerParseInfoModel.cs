namespace CombatAnalysis.Core.Models;

public class PlayerParseInfoModel
{
    public int SpecId { get; set; }

    public int ClassId { get; set; }

    public int BossId { get; set; }

    public int Difficult { get; set; }

    public int DamageEfficiency { get; set; }

    public int HealEfficiency { get; set; }

    public int CombatPlayerId { get; set; }
}
