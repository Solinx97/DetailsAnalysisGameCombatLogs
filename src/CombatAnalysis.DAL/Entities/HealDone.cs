using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Entities;

[Index(nameof(CombatPlayerId))]
public class HealDone
{
    public int Id { get; set; }

    public int ValueWithOverheal { get; set; }

    public string Time { get; set; }

    public int Overheal { get; set; }

    public int Value { get; set; }

    public string FromPlayer { get; set; }

    public string ToPlayer { get; set; }

    public string SpellOrItem { get; set; }

    public bool IsCrit { get; set; }

    public bool IsFullOverheal { get; set; }

    public int CombatPlayerId { get; set; }
}
