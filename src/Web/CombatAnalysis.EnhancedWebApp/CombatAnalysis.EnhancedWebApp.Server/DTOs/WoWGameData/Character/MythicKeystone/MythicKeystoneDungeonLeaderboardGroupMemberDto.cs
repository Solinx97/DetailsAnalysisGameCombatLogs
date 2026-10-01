namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardGroupMemberDto
{
    public WoWGameDataCharacterDto Character { get; set; }

    public WoWGameDataTypeDto Faction { get; set; }

    public WoWGameDataEntityDto Specialization { get; set; }
}
