namespace CombatAnalysis.BL.DTO;

public class HealDoneDto : CombatDataBase
{
    public int Id { get; set; }

    public int ValueWithOverheal { get; set; }

    public TimeSpan Time { get; set; }

    public int Overheal { get; set; }

    public int Value { get; set; }

    public string FromPlayer { get; set; }

    public string ToPlayer { get; set; }

    public string SpellOrItem { get; set; }

    public string DamageAbsorbed { get; set; }

    public bool IsCrit { get; set; }

    public bool IsFullOverheal { get; set; }

    public bool IsAbsorbed { get; set; }
}
