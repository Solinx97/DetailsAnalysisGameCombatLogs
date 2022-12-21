namespace CombatAnalysis.CombatParser.Core;

internal static class CombatLogConsts
{
    #region Common

    public static string CombatLogVersion
    {
        get
        {
            return "COMBAT_LOG_VERSION";
        }
    }

    public static string EncounterStart
    {
        get
        {
            return "ENCOUNTER_START";
        }
    }

    public static string EncounterEnd
    {
        get
        {
            return "ENCOUNTER_END";
        }
    }

    public static string ZoneChange
    {
        get
        {
            return "ZONE_CHANGE";
        }
    }

    public static string CombatantInfo
    {
        get
        {
            return "COMBATANT_INFO";
        }
    }

    public static string UnitDied
    {
        get
        {
            return "UNIT_DIED";
        }
    }

    #endregion

    #region DamageDoneInformations

    public static string SpellDamage
    {
        get
        {
            return "SPELL_DAMAGE";
        }
    }

    public static string SwingDamage
    {
        get
        {
            return "SWING_DAMAGE";
        }
    }

    public static string SpellPeriodicDamage
    {
        get
        {
            return "SPELL_PERIODIC_DAMAGE";
        }
    }

    public static string SwingMissed
    {
        get
        {
            return "SWING_MISSED";
        }
    }

    public static string DamageShieldMissed
    {
        get
        {
            return "DAMAGE_SHIELD_MISSED";
        }
    }

    public static string RangeDamage
    {
        get
        {
            return "RANGE_DAMAGE";
        }
    }

    public static string SpellMissed
    {
        get
        {
            return "SPELL_MISSED";
        }
    }

    public static string SwingDamageLanded
    {
        get
        {
            return "SWING_DAMAGE_LANDED";
        }
    }

    #endregion

    #region HealDone

    public static string SpellHeal
    {
        get
        {
            return "SPELL_HEAL";
        }
    }

    public static string SpellPeriodicHeal
    {
        get
        {
            return "SPELL_PERIODIC_HEAL";
        }
    }

    #endregion

    #region ResourceRecovery

    public static string SpellPeriodicEnergize
    {
        get
        {
            return "SPELL_PERIODIC_ENERGIZE";
        }
    }

    public static string SpellEnergize
    {
        get
        {
            return "SPELL_ENERGIZE";
        }
    }

    #endregion
}
