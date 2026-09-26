namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;

public class CharacterReputationDto
{
    public WoWGameDataEntityDto Faction { get; set; }

    public CharacterReputationStandingDto Standing { get; set; }
}
