using CombatParser.Application.Interfaces;

namespace CombatParser.Application.DTOs;

public class CombatPlayerDto
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public IPlayerStatsDto Stats { get; set; }

    public SpecializationScoreDto Score { get; set; }

    public PlayerDto Player { get; set; }

    public string PlayerId { get; set; }

    public UnitDto Unit { get; set; }

    public string UnitId { get; set; }

    public int CombatId { get; set; }

    public string UnitGameId { get; set; }
}
