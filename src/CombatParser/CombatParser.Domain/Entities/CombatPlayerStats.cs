namespace CombatParser.Domain.Entities;

public record CombatPlayerStats
{
    public const int TALENTS_MAX_LENGTH = 128;

    private CombatPlayerStats() { }

    public CombatPlayerStats(int strength, int agility, int intelligence, int stamina, int spirit,
        int dodge, int parry, int crit, int haste, int hit, 
        int expertise, int armor, string talents, int combatPlayerId)
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
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; }

    public int Strength { get; }

    public int Agility { get; }

    public int Intelligence { get; }

    public int Stamina { get; }

    public int Spirit { get; }

    public int Dodge { get; }

    public int Parry { get; }

    public int Crit { get; }

    public int Haste { get; }

    public int Hit { get; }

    public int Expertise { get; }

    public int Armor { get; }

    public string Talents { get; } = string.Empty;

    public int CombatPlayerId { get; }
}
