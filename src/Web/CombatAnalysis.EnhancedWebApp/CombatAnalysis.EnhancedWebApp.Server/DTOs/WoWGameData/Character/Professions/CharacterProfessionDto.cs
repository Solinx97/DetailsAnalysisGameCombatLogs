namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Professions;

public class CharacterProfessionDto
{
    public WoWGameDataEntityDto Profession { get; set; }

    public CharacterProfessionTierDto[] Tiers { get; set; }
}
