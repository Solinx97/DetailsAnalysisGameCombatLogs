namespace CombatAnalysis.CombatParser.Core;

internal static class CombatLogKeyWords
{
    #region Common

    public static string CombatLogVersion { get; } = "COMBAT_LOG_VERSION";

    public static string EncounterStart { get; } = "ENCOUNTER_START";

    public static string EncounterEnd { get; } = "ENCOUNTER_END";

    public static string ZoneChange { get; } = "ZONE_CHANGE";

    public static string CombatantInfo { get; } = "COMBATANT_INFO";

    public static string SpellSummon { get; } = "SPELL_SUMMON";

    public static string UnitDied { get; } = "UNIT_DIED";

    public static string Creature { get; } = "Creature";

    public static string Pet { get; } = "Pet";

    public static string Boss { get; } = "Vehicle";

    public static string Player { get; } = "Player";

    #endregion

    #region Auras

    public static string AuraApplied { get; } = "SPELL_AURA_APPLIED";

    public static string AuraRemoved { get; } = "SPELL_AURA_REMOVED";

    public static string AuraAppliedDose { get; } = "SPELL_AURA_APPLIED_DOSE";

    public static string Debuff { get; } = "DEBUFF";

    #endregion

    #region Damage done

    public static string SpellDamage { get; } = "SPELL_DAMAGE";

    public static string SwingDamage { get; } = "SWING_DAMAGE";

    public static string SpellPeriodicDamage { get; } = "SPELL_PERIODIC_DAMAGE";

    public static string SwingMissed { get; } = "SWING_MISSED";

    public static string DamageShieldMissed { get; } = "DAMAGE_SHIELD_MISSED";

    public static string RangeDamage { get; } = "RANGE_DAMAGE";

    public static string SpellCastSuccess { get; } = "SPELL_CAST_SUCCESS";

    public static string SpellMissed { get; } = "SPELL_MISSED";

    public static string SwingDamageLanded { get; } = "SWING_DAMAGE_LANDED";

    public static string MeleeDamage { get; } = "Melee";

    public static string IsCrit { get; } = "1";

    #endregion

    #region Tank ability

    public static string Resist { get; } = "RESIST";

    public static string Immune { get; } = "IMMUNE";

    public static string Parry { get; } = "PARRY";

    public static string Dodge { get; } = "DODGE";

    public static string Miss { get; } = "MISS";

    public static string Absorb { get; } = "ABSORB";

    public static string IsCrushing { get; } = "1";

    #endregion

    #region Heal done

    public static string SpellHeal { get; } = "SPELL_HEAL";

    public static string SpellPeriodicHeal { get; } = "SPELL_PERIODIC_HEAL";

    public static string SpellAbsorbed { get; } = "SPELL_ABSORBED";

    #endregion

    #region Resources recovery

    public static string SpellPeriodicEnergize { get; } = "SPELL_PERIODIC_ENERGIZE";

    public static string SpellEnergize { get; } = "SPELL_ENERGIZE";

    #endregion
}
