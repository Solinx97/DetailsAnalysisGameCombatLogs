namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Data;

public class SearchItemDataDto : WoWGameDataEntityDto
{
    public int Level { get; set; }

    public int RequiredLevel { get; set; }

    public long SellPrice { get; set; }

    public WoWGameDataEntityDto ItemClass { get; set; }

    public WoWGameDataEntityDto ItemSubclass { get; set; }

    public WoWGameDataTypeDto Quality { get; set; }

    public bool IsEquippable { get; set; }

    public int PurchaseQuantity { get; set; }

    public int MaxCount { get; set; }

    public bool IsStackable { get; set; }

    public string Name { get; set; }

    public long PurchasePrice { get; set; }
}
