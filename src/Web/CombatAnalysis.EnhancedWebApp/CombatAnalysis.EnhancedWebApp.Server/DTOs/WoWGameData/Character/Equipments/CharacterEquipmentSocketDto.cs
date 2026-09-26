namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentSocketDto
{
    public WoWGameDataTypeDto Type { get; set; }

    public WoWGameDataEntityDto Item { get; set; }

    public string DisplayString { get; set; }
}
