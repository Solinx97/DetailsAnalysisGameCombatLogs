using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentModel
{
    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("enchantments")]
    public CharacterEquipmentEnchantmentModel[] Enchantments { get; set; }

    [JsonPropertyName("sockets")]
    public CharacterEquipmentSocketModel[] Sockets { get; set; }

    [JsonPropertyName("slot")]
    public WoWGameDataTypeModel Slot { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("context")]
    public int Context { get; set; }

    [JsonPropertyName("bonus_list")]
    public int[] BonusList { get; set; }

    [JsonPropertyName("quality")]
    public WoWGameDataTypeModel Quality { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("modified_appearance_id")]
    public int ModifiedAppearanceId { get; set; }

    [JsonPropertyName("item_class")]
    public WoWGameDataEntityModel ItemClass { get; set; }

    [JsonPropertyName("item_subclass")]
    public WoWGameDataEntityModel ItemSubclass { get; set; }

    [JsonPropertyName("inventory_type")]
    public WoWGameDataTypeModel InventoryType { get; set; }

    [JsonPropertyName("binding")]
    public WoWGameDataTypeModel Binding { get; set; }

    [JsonPropertyName("unique_equipped")]
    public string? UniqueEquipped { get; set; }

    [JsonPropertyName("armor")]
    public CharacterEquipmentStatModel? Armor { get; set; }

    [JsonPropertyName("stats")]
    public CharacterEquipmentStatModel[] Stats { get; set; }

    [JsonPropertyName("spells")]
    public CharacterEquipmentSpellModel[]? Spells { get; set; }

    [JsonPropertyName("sell_price")]
    public WoWGameDataCurrencyModel SellPrice { get; set; }

    [JsonPropertyName("requirements")]
    public CharacterEquipmentRequirementsModel Requirements { get; set; }

    [JsonPropertyName("set")]
    public CharacterEquipmentSetModel Set { get; set; }

    [JsonPropertyName("level")]
    public WoWGameDataValueModel Level { get; set; }

    [JsonPropertyName("transmog")]
    public CharacterEquipmentTransmogModel Transmog { get; set; }

    [JsonPropertyName("durability")]
    public WoWGameDataValueModel Durability { get; set; }

    [JsonPropertyName("is_subclass_hidden")]
    public bool? IsSubclassHidden { get; set; }

    [JsonPropertyName("name_description")]
    public WoWGameDataItemDisplayModel NameDescription { get; set; }

    [JsonPropertyName("modified_crafting_stat")]
    public CharacterEquipmentCraftingStatModel[]? ModifiedCraftingStat { get; set; }
}
