namespace CombatAnalysis.BL.DTO;

public class ResourceRecoveryDto : CombatDataBase
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string SpellOrItem { get; set; }
}
