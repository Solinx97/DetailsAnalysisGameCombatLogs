namespace CombatAnalysis.CombatParser.Entities;

public class DamageTaken : CombatDataBase
{
    public string Spell { get; set; }

    public int Value { get; set; }

    public int ActualValue { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Mitigated { get; set; }

    public int DamageTakenType { get; set; }
}
