namespace CombatAnalysis.BL.DTO;

public class ResourceRecoveryDto : BasePlayerInfoDto
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string SpellOrItem { get; set; }

    public override int CombatPlayerId { get; set; }
}
