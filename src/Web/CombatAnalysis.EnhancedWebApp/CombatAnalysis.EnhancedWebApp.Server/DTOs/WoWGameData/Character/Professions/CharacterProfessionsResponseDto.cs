namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Professions;

public class CharacterProfessionsResponseDto
{
    public CharacterProfessionDto[] Primaries { get; set; }

    public CharacterProfessionDto[] Secondaries { get; set; }
}
