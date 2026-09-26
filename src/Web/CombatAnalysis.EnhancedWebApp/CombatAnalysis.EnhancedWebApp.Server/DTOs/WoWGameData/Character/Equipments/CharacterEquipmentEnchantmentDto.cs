namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentEnchantmentDto
{
    public int Id { get; set; }

    public string DisplayString { get; set; }

    public CharacterEquipmentSlotDto Slot { get; set; }
}
