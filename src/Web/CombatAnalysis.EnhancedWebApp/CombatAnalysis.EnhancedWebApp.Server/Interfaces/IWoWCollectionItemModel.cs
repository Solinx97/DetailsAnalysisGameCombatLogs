using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces;

public interface IWoWCollectionItemModel
{
    WoWGameDataEntityModel Item { get; }

    bool IsFavorite { get; }
}
