namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentTransmogDto
{
    public WoWGameDataEntityDto Item { get; set; }

    public string DisplayString { get; set; }

    public int ItemModifiedAppearanceId { get; set; }
}
