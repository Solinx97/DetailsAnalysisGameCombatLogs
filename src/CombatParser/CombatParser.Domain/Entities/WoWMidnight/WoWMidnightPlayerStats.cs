using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.WoWMidnight;

public class WoWMidnightPlayerStats : CombatPlayerDataBase, IPlayerStats
{
    public const int TALENTS_MAX_LENGTH = 128;

    private WoWMidnightPlayerStats() { }

    public WoWMidnightPlayerStats(int strength, int agility, int intelligence, int stamina,
        int dodge, int parry, int block, int crit, int haste, int mastery, int versality, 
        int lifesteal, int avoidance, int movement, int armor, string talents, int combatPlayerId)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(strength, nameof(strength));
        ArgumentOutOfRangeException.ThrowIfNegative(agility, nameof(agility));
        ArgumentOutOfRangeException.ThrowIfNegative(intelligence, nameof(intelligence));
        ArgumentOutOfRangeException.ThrowIfNegative(stamina, nameof(stamina));
        ArgumentOutOfRangeException.ThrowIfNegative(dodge, nameof(dodge));
        ArgumentOutOfRangeException.ThrowIfNegative(parry, nameof(parry));
        ArgumentOutOfRangeException.ThrowIfNegative(block, nameof(block));
        ArgumentOutOfRangeException.ThrowIfNegative(crit, nameof(crit));
        ArgumentOutOfRangeException.ThrowIfNegative(haste, nameof(haste));
        ArgumentOutOfRangeException.ThrowIfNegative(mastery, nameof(mastery));
        ArgumentOutOfRangeException.ThrowIfNegative(versality, nameof(versality));
        ArgumentOutOfRangeException.ThrowIfNegative(lifesteal, nameof(lifesteal));
        ArgumentOutOfRangeException.ThrowIfNegative(avoidance, nameof(avoidance));
        ArgumentOutOfRangeException.ThrowIfNegative(movement, nameof(movement));
        ArgumentOutOfRangeException.ThrowIfNegative(armor, nameof(armor));

        Strength = strength;
        Agility = agility;
        Intelligence = intelligence;
        Stamina = stamina;
        Dodge = dodge;
        Parry = parry;
        Block = block;
        Crit = crit;
        Haste = haste;
        Mastery = mastery;
        Versality = versality;
        Lifesteal = lifesteal;
        Avoidance = avoidance;
        Movement = movement;
        Armor = armor;
        Talents = talents;
        CombatPlayerId = combatPlayerId;
    }

    public int Strength { get; protected set; }

    public int Agility { get; protected set; }

    public int Intelligence { get; protected set; }

    public int Stamina { get; protected set; }

    public int Dodge { get; protected set; }

    public int Parry { get; protected set; }

    public int Block { get; protected set; }

    public int Crit { get; protected set; }

    public int Haste { get; protected set; }

    public int Armor { get; protected set; }

    public int Mastery { get; private set; }

    public int Versality { get; private set; }

    public int Lifesteal { get; private set; }

    public int Avoidance { get; private set; }

    public int Movement { get; private set; }

    public string Talents { get; protected set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; protected set; }
}
