namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Professions;

public class CharacterProfessionTierDto
{
    public int SkillPoints { get; set; }

    public int MaxSkillPoints { get; set; }

    public WoWGameDataEntityDto Tier { get; set; }

    public WoWGameDataEntityDto[] KnownRecipes { get; set; }
}
