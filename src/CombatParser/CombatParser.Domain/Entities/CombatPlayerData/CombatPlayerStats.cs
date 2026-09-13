using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerStats : CombatPlayerDataBase
{
    public const int TALENTS_MAX_LENGTH = 128;

    private CombatPlayerStats() { }

    private CombatPlayerStats(int strength, int agility, int intelligence, int stamina, int spirit,
        int dodge, int parry, int crit, int haste, int hit, 
        int expertise, int armor, string talents)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Stamina = stamina;
        Spirit = spirit;
        Dodge = dodge;
        Parry = parry;
        Crit = crit;
        Haste = haste;
        Hit = hit;
        Expertise = expertise;
        Armor = armor;
        Talents = talents;
    }

    public int Strength { get; private set; }

    public int Agility { get; private set; }

    public int Intelligence { get; private set; }

    public int Stamina { get; private set; }

    public int Spirit { get; private set; }

    public int Dodge { get; private set; }

    public int Parry { get; private set; }

    public int Crit { get; private set; }

    public int Haste { get; private set; }

    public int Hit { get; private set; }

    public int Expertise { get; private set; }

    public int Armor { get; private set; }

    public string Talents { get; private set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; private set; }

    public static CombatPlayerStats Create(int strength, int agility, int intelligence, int stamina, int spirit,
        int dodge, int parry, int crit, int haste, int hit,
        int expertise, int armor, string talents)
    {
        ArgumentException.ThrowIfNullOrEmpty(talents, nameof(talents));
        ArgumentOutOfRangeException.ThrowIfNegative(strength, nameof(strength));
        ArgumentOutOfRangeException.ThrowIfNegative(agility, nameof(agility));
        ArgumentOutOfRangeException.ThrowIfNegative(intelligence, nameof(intelligence));
        ArgumentOutOfRangeException.ThrowIfNegative(stamina, nameof(stamina));
        ArgumentOutOfRangeException.ThrowIfNegative(spirit, nameof(spirit));
        ArgumentOutOfRangeException.ThrowIfNegative(dodge, nameof(dodge));
        ArgumentOutOfRangeException.ThrowIfNegative(parry, nameof(parry));
        ArgumentOutOfRangeException.ThrowIfNegative(crit, nameof(crit));
        ArgumentOutOfRangeException.ThrowIfNegative(haste, nameof(haste));
        ArgumentOutOfRangeException.ThrowIfNegative(hit, nameof(hit));
        ArgumentOutOfRangeException.ThrowIfNegative(expertise, nameof(expertise));
        ArgumentOutOfRangeException.ThrowIfNegative(armor, nameof(armor));

        return new CombatPlayerStats(strength, agility, intelligence, stamina, spirit,
            dodge, parry, crit, haste, hit,
            expertise, armor, talents);
    }
}
