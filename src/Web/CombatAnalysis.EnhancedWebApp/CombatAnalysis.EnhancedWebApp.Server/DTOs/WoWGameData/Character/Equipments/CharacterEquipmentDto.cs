namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentDto
{
    public WoWGameDataEntityDto Item { get; set; }

    public CharacterEquipmentEnchantmentDto[] Enchantments { get; set; }

    public CharacterEquipmentSocketDto[] Sockets { get; set; }

    public WoWGameDataTypeDto Slot { get; set; }

    public int Quantity { get; set; }

    public int Context { get; set; }

    public int[] BonusList { get; set; }

    public WoWGameDataTypeDto Quality { get; set; }

    public string Name { get; set; }

    public int ModifiedAppearanceId { get; set; }

    public WoWGameDataEntityDto ItemClass { get; set; }

    public WoWGameDataEntityDto ItemSubclass { get; set; }

    public WoWGameDataTypeDto InventoryType { get; set; }

    public WoWGameDataTypeDto Binding { get; set; }

    public string? UniqueEquipped { get; set; }

    public CharacterEquipmentStatDto? Armor { get; set; }

    public CharacterEquipmentStatDto[] Stats { get; set; }

    public CharacterEquipmentSpellDto[]? Spells { get; set; }

    public WoWGameDataCurrencyDto SellPrice { get; set; }

    public CharacterEquipmentRequirementsDto Requirements { get; set; }

    public CharacterEquipmentSetDto Set { get; set; }

    public WoWGameDataValueDto Level { get; set; }

    public CharacterEquipmentTransmogDto Transmog { get; set; }

    public WoWGameDataValueDto Durability { get; set; }

    public bool? IsSubclassHidden { get; set; }

    public WoWGameDataItemDisplayDto NameDescription { get; set; }

    public CharacterEquipmentCraftingStatDto[]? ModifiedCraftingStat { get; set; }
}
