namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;

public class CharacterDto
{
    public string Name { get; set; }

    public CharacterGenderDto Gender { get; set; }

    public FactionDto Faction { get; set; }

    public CharacterRaceDto Race { get; set; }

    public CharacterClassDto Class { get; set; }

    public CharacterSpecializationDto ActiveSpec { get; set; }

    public RealmDto Realm { get; set; }

    public GuildDto Guild { get; set; }

    public int Level { get; set; }

    public int Experience { get; set; }

    public int AchievementPoints { get; set; }

    public long LastLogin { get; set; }

    public int AverageItemLevel { get; set; }

    public int EquippedItemLevel { get; set; }

    public CharacterTitleDto ActiveTitle { get; set; }

    public bool IsRemix { get; set; }

    public string NameSearch { get; set; }
}
