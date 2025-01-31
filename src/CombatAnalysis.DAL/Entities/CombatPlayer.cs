using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class CombatPlayer : IEntity
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string PlayerId { get; set; }

    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int CombatId { get; set; }
}
