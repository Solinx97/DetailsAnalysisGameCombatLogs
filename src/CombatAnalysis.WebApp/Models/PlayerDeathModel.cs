namespace CombatAnalysis.WebApp.Models;

public class PlayerDeathModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; }

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
