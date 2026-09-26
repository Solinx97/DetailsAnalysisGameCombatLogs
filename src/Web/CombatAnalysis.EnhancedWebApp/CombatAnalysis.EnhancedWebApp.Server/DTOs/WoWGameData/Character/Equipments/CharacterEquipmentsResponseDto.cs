namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentsResponseDto
{
    public CharacterEquipmentDto[] EquippedItems { get; set; }

    public CharacterEquipmentSetDto[] EquippedItemSets { get; set; }
}
