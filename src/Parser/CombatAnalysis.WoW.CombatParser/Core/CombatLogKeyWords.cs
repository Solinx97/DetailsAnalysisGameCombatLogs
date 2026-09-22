namespace CombatAnalysis.WoW.CombatParser.Core;

public static class CombatLogKeyWords
{
    #region Common

    public static TimeSpan MinCombatDuration { get; } = TimeSpan.Parse("00:00:20");

    public const string NULL_VALUE = "nil";

    public const string COMBAT_LOG_VERSION = "COMBAT_LOG_VERSION";

    public const string ENCOUNTER_START = "ENCOUNTER_START";

    public const string ENCOUNTER_END = "ENCOUNTER_END";

    public const string ZONE_CHANGE = "ZONE_CHANGE";

    public const string COMBATANT_INFO = "COMBATANT_INFO";

    public const string SPELL_SUMMON = "SPELL_SUMMON";

    public const string UNIT_DIED = "UNIT_DIED";

    public const string CREATURE = "Creature";

    public const string PET = "Pet";

    public const string VEHICLE = "Vehicle";

    public const string PLAYER = "Player";

    #endregion

    #region Casts

    public const string SPELL_CAST_START = "SPELL_CAST_START";

    public const string SPELL_CAST_SUCCESS = "SPELL_CAST_SUCCESS";

    #endregion

    #region Auras

    public const string SPELL_AURA_APPLIED = "SPELL_AURA_APPLIED";

    public const string SPELL_AURA_APPLIED_DOSE = "SPELL_AURA_APPLIED_DOSE";

    public const string DEBUFF = "DEBUFF";

    #endregion

    #region Damage done

    public const string SPELL_DAMAGE = "SPELL_DAMAGE";

    public const string SWING_DAMAGE = "SWING_DAMAGE,";

    public const string SPELL_PERIODIC_DAMAGE = "SPELL_PERIODIC_DAMAGE";

    public const string SWING_DAMAGE_LANDED = "SWING_DAMAGE_LANDED";

    public const string MELEE = "Melee";

    public const string SINGLE_TARGET = "ST";

    public const string AOE = "AOE";

    public const string CRIT = "1";

    #endregion

    #region Tank ability

    public const string RESIST = "RESIST";

    public const string IMMUNE = "IMMUNE";

    public const string PARRY = "PARRY";

    public const string DODGE = "DODGE";

    public const string MISS = "MISS";

    public const string ABSORB = "ABSORB";

    public const string CRUSHING = "1";

    #endregion

    #region Damage missed

    public const string SWING_MISSED = "SWING_MISSED";

    public const string DAMAGE_SHIELD_MISSED = "DAMAGE_SHIELD_MISSED";

    public const string SPELL_MISSED = "SPELL_MISSED";

    #endregion
}
