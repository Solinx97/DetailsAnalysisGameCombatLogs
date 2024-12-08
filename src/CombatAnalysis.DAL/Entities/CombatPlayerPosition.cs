using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class CombatPlayerPosition : ICombatPlayerEntity
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public int CombatId { get; set; }

    public int CombatPlayerId { get; set; }
}