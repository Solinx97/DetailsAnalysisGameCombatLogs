namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneMemberDto
{
    public WoWGameDataCharacterDto Character { get; set; }

    public WoWGameDataEntityDto Specialization { get; set; }

    public WoWGameDataEntityDto Race { get; set; }

    public int EquippedItemLevel { get; set; }
}
