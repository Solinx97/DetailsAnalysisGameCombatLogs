namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;

public class CharacterDto : WoWGameDataEntityDto
{
    public WoWRealmDto Realm { get; set; }

    public WoWGameDataEntityDto PlayableClass { get; set; }

    public WoWGameDataEntityDto PlayableRace { get; set; }

    public WoWGameDataTypeDto Gender { get; set; }

    public WoWGameDataTypeDto Faction { get; set; }

    public int Level { get; set; }
}
