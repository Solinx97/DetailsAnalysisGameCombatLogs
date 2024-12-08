namespace CombatAnalysis.BL.DTO;

public class PlayerDeathDto : CombatDataBase
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string LastHitSpellOrItem { get; set; }

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }
}
