namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentStatDto
{
    public WoWGameDataTypeDto? Type { get; set; }

    public int Value { get; set; }

    public bool? IsNegated { get; set; }

    public WoWGameDataItemDisplayDto Display { get; set; }
}
