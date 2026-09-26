namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;

public class CharacterEquipmentSpellDto
{
    public WoWGameDataEntityDto Spell { get; set; }

    public string Description { get; set; }

    public WoWGameDataColorDto Color { get; set; }
}
