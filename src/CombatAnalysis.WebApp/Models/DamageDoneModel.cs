namespace CombatAnalysis.WebApp.Models;

public class DamageDoneModel
{
    public int Id { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public int DamageType { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public bool IsPet { get; set; }

    public int CombatPlayerId { get; set; }
}
