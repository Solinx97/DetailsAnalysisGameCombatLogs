namespace CombatAnalysis.WebApp.Models;

public class ResourceRecoveryModel
{
    public int Id { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int CombatPlayerId { get; set; }
}
