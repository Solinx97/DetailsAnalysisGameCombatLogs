namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentSetDto
{
    public WoWGameDataEntityDto ItemSet { get; set; }

    public CharacterEquipmentSetItemDto[] Items { get; set; }

    public CharacterEquipmentSetEffectDto[] Effects { get; set; }

    public string DisplayString { get; set; }
}
