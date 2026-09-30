namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public interface IWoWCollectionItemModel
{
    WoWGameDataEntityModel Item { get; }

    bool IsFavorite { get; }
}
