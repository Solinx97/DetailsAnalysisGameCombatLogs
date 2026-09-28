using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterStatsModel
{
    [JsonPropertyName("health")]
    public long Health { get; set; }

    [JsonPropertyName("power")]
    public int Power { get; set; }

    [JsonPropertyName("power_type")]
    public WoWGameDataEntityModel PowerType { get; set; }

    [JsonPropertyName("speed")]
    public CharacterStatPowerRatingModel Speed { get; set; }

    [JsonPropertyName("strength")]
    public CharacterStatPowerModel Strength { get; set; }

    [JsonPropertyName("agility")]
    public CharacterStatPowerModel Agility { get; set; }

    [JsonPropertyName("intellect")]
    public CharacterStatPowerModel Intellect { get; set; }

    [JsonPropertyName("stamina")]
    public CharacterStatPowerModel Stamina { get; set; }

    [JsonPropertyName("melee_crit")]
    public CharacterStatPowerRatingModel MeleeCrit { get; set; }

    [JsonPropertyName("melee_haste")]
    public CharacterStatPowerRatingModel MeleeHaste { get; set; }

    [JsonPropertyName("mastery")]
    public CharacterStatPowerRatingModel Mastery { get; set; }

    [JsonPropertyName("bonus_armor")]
    public int BonusArmor { get; set; }

    [JsonPropertyName("lifesteal")]
    public CharacterStatPowerRatingModel Lifesteal { get; set; }

    [JsonPropertyName("versatility")]
    public double Versatility { get; set; }

    [JsonPropertyName("versatility_damage_done_bonus")]
    public double VersatilityDamageDoneBonus { get; set; }

    [JsonPropertyName("versatility_healing_done_bonus")]
    public double VersatilityHealingDoneBonus { get; set; }

    [JsonPropertyName("versatility_damage_taken_bonus")]
    public double VersatilityDamageTakenBonus { get; set; }

    [JsonPropertyName("avoidance")]
    public CharacterStatPowerRatingModel Avoidance { get; set; }

    [JsonPropertyName("attack_power")]
    public int AttackPower { get; set; }

    [JsonPropertyName("main_hand_damage_min")]
    public double MainHandDamageMin { get; set; }

    [JsonPropertyName("main_hand_damage_max")]
    public double MainHandDamageMax { get; set; }

    [JsonPropertyName("main_hand_speed")]
    public double MainHandSpeed { get; set; }

    [JsonPropertyName("main_hand_dps")]
    public double MainHandDps { get; set; }

    [JsonPropertyName("off_hand_damage_min")]
    public double OffHandDamageMin { get; set; }

    [JsonPropertyName("off_hand_damage_max")]
    public double OffHandDamageMax { get; set; }

    [JsonPropertyName("off_hand_speed")]
    public double OffHandSpeed { get; set; }

    [JsonPropertyName("off_hand_dps")]
    public double OffHandDps { get; set; }

    [JsonPropertyName("spell_power")]
    public int SpellPower { get; set; }

    [JsonPropertyName("spell_penetration")]
    public int SpellPenetration { get; set; }

    [JsonPropertyName("spell_crit")]
    public CharacterStatPowerRatingModel SpellCrit { get; set; }

    [JsonPropertyName("mana_regen")]
    public double ManaRegen { get; set; }

    [JsonPropertyName("mana_regen_combat")]
    public double ManaRegenCombat { get; set; }

    [JsonPropertyName("armor")]
    public CharacterStatPowerModel Armor { get; set; }

    [JsonPropertyName("dodge")]
    public CharacterStatPowerRatingModel Dodge { get; set; }

    [JsonPropertyName("parry")]
    public CharacterStatPowerRatingModel Parry { get; set; }

    [JsonPropertyName("block")]
    public CharacterStatPowerRatingModel Block { get; set; }

    [JsonPropertyName("ranged_crit")]
    public CharacterStatPowerRatingModel RangedCrit { get; set; }

    [JsonPropertyName("ranged_haste")]
    public CharacterStatPowerRatingModel RangedHaste { get; set; }

    [JsonPropertyName("spell_haste")]
    public CharacterStatPowerRatingModel SpellHaste { get; set; }
}
