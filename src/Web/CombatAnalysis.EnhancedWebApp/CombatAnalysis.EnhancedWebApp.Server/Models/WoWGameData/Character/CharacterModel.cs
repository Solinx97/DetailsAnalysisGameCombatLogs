using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class CharacterModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("gender")]
    public CharacterGenderModel Gender { get; set; }

    [JsonPropertyName("faction")]
    public FactionModel Faction { get; set; }

    [JsonPropertyName("race")]
    public CharacterRaceModel Race { get; set; }

    [JsonPropertyName("character_class")]
    public CharacterClassModel Class { get; set; }

    [JsonPropertyName("active_spec")]
    public CharacterSpecializationModel ActiveSpec { get; set; }

    [JsonPropertyName("realm")]
    public RealmModel Realm { get; set; }

    [JsonPropertyName("guild")]
    public GuildModel Guild { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("experience")]
    public int Experience { get; set; }

    [JsonPropertyName("achievement_points")]
    public int AchievementPoints { get; set; }

    [JsonPropertyName("last_login_timestamp")]
    public long LastLogin { get; set; }

    [JsonPropertyName("average_item_level")]
    public int AverageItemLevel { get; set; }

    [JsonPropertyName("equipped_item_level")]
    public int EquippedItemLevel { get; set; }

    [JsonPropertyName("active_title")]
    public CharacterTitleModel ActiveTitle { get; set; }

    [JsonPropertyName("is_remix")]
    public bool IsRemix { get; set; }

    [JsonPropertyName("name_search")]
    public string NameSearch { get; set; }
}
