namespace CombatAnalysis.CombatParserAPI.Models;

public class ResourceRecoveryModel
{
    public int Id { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public int CombatPlayerId { get; set; }
}
