using CombatAnalysis.BL.Interfaces.Entity;

namespace CombatAnalysis.BL.DTO;

public class CombatPlayerPositionDto : ICombatPlayerEntity
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }

    public int CombatPlayerId { get; set; }
}
