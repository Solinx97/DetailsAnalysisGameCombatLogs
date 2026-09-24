namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneMemberDto
{
    public DungeonCharacterDto Character { get; set; }

    public CharacterSpecializationDto Specialization { get; set; }

    public CharacterRaceDto Race { get; set; }

    public int EquippedItemLevel { get; set; }
}
