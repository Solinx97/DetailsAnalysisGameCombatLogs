using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.DAL.Entities;

public class PlayerDeath : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; }

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
