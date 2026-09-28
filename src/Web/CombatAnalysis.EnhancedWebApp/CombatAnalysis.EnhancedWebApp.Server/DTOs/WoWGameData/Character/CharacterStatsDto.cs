namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;

public class CharacterStatsDto
{
    public long Health { get; set; }

    public int Power { get; set; }

    public WoWGameDataEntityDto PowerType { get; set; }

    public CharacterStatPowerRatingDto Speed { get; set; }

    public CharacterStatPowerDto Strength { get; set; }

    public CharacterStatPowerDto Agility { get; set; }

    public CharacterStatPowerDto Intellect { get; set; }

    public CharacterStatPowerDto Stamina { get; set; }

    public CharacterStatPowerRatingDto MeleeCrit { get; set; }

    public CharacterStatPowerRatingDto MeleeHaste { get; set; }

    public CharacterStatPowerRatingDto Mastery { get; set; }

    public int BonusArmor { get; set; }

    public CharacterStatPowerRatingDto Lifesteal { get; set; }

    public double Versatility { get; set; }

    public double VersatilityDamageDoneBonus { get; set; }

    public double VersatilityHealingDoneBonus { get; set; }

    public double VersatilityDamageTakenBonus { get; set; }

    public CharacterStatPowerRatingDto Avoidance { get; set; }

    public int AttackPower { get; set; }

    public double MainHandDamageMin { get; set; }

    public double MainHandDamageMax { get; set; }

    public double MainHandSpeed { get; set; }

    public double MainHandDps { get; set; }

    public double OffHandDamageMin { get; set; }

    public double OffHandDamageMax { get; set; }

    public double OffHandSpeed { get; set; }

    public double OffHandDps { get; set; }

    public int SpellPower { get; set; }

    public int SpellPenetration { get; set; }

    public CharacterStatPowerRatingDto SpellCrit { get; set; }

    public double ManaRegen { get; set; }

    public double ManaRegenCombat { get; set; }

    public CharacterStatPowerDto Armor { get; set; }

    public CharacterStatPowerRatingDto Dodge { get; set; }

    public CharacterStatPowerRatingDto Parry { get; set; }

    public CharacterStatPowerRatingDto Block { get; set; }

    public CharacterStatPowerRatingDto RangedCrit { get; set; }

    public CharacterStatPowerRatingDto RangedHaste { get; set; }

    public CharacterStatPowerRatingDto SpellHaste { get; set; }
}
